import { defineStore } from 'pinia'
import { ref } from 'vue'
import { taskService } from '@/services/task.service'
import type { TaskItem, CreateTaskRequest, UpdateTaskRequest } from '@/types/task'

export const useTaskStore = defineStore('task', () => {
  const tasks = ref<TaskItem[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)

  async function fetchTasksByBoard(boardId: number) {
    try {
      loading.value = true
      error.value = null
      tasks.value = await taskService.getByBoardId(boardId)
    } catch (err: any) {
      error.value = err.response?.data?.message || 'Failed to fetch tasks'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function createTask(data: CreateTaskRequest) {
    try {
      loading.value = true
      error.value = null
      const newTask = await taskService.create(data)
      tasks.value.push(newTask)
      return newTask
    } catch (err: any) {
      error.value = err.response?.data?.message || 'Failed to create task'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function updateTask(data: UpdateTaskRequest) {
    try {
      loading.value = true
      error.value = null
      await taskService.update(data)

      const task = tasks.value.find(t => t.id === data.id)
      if (task) {
        task.title = data.title
        task.description = data.description
        if (data.assignedUserId !== undefined) {
          task.assignedUserId = data.assignedUserId
        }
      }
    } catch (err: any) {
      error.value = err.response?.data?.message || 'Failed to update task'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function updateTaskStatus(id: number, status: number) {
    // Find the task's board ID for error recovery
    const task = tasks.value.find(t => t.id === id)
    const boardId = task?.boardId

    try {
      loading.value = true
      error.value = null
      await taskService.updateStatus(id, status)

      // Update local state on success
      if (task) {
        task.status = status as any
      }
    } catch (err: any) {
      error.value = err.response?.data?.message || 'Failed to update task status'
      // Re-fetch tasks to restore correct state after failed drag-drop
      if (boardId) {
        try {
          tasks.value = await taskService.getByBoardId(boardId)
        } catch { /* ignore refetch errors */ }
      }
      throw err
    } finally {
      loading.value = false
    }
  }

  async function deleteTask(id: number) {
    try {
      loading.value = true
      error.value = null
      await taskService.delete(id)
      tasks.value = tasks.value.filter(t => t.id !== id)
    } catch (err: any) {
      error.value = err.response?.data?.message || 'Failed to delete task'
      throw err
    } finally {
      loading.value = false
    }
  }

  return {
    tasks,
    loading,
    error,
    fetchTasksByBoard,
    createTask,
    updateTask,
    updateTaskStatus,
    deleteTask
  }
})
