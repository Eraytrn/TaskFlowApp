import { api } from './api'
import type { TaskItem, CreateTaskRequest, UpdateTaskRequest } from '@/types/task'

export const taskService = {
  async getByBoardId(boardId: number): Promise<TaskItem[]> {
    const response = await api.get<TaskItem[]>(`/TaskItems/board/${boardId}`)
    return response.data
  },

  async getById(id: number): Promise<TaskItem> {
    const response = await api.get<TaskItem>(`/TaskItems/${id}`)
    return response.data
  },

  async create(data: CreateTaskRequest): Promise<TaskItem> {
    const response = await api.post<TaskItem>('/TaskItems', data)
    return response.data
  },

  async update(data: UpdateTaskRequest): Promise<void> {
    await api.put(`/TaskItems/${data.id}`, data)
  },

  async updateStatus(id: number, status: number): Promise<void> {
    await api.patch(`/TaskItems/${id}/status`, { id, newStatus: status })
  },

  async delete(id: number): Promise<void> {
    await api.delete(`/TaskItems/${id}`)
  },

  async assignLabel(taskId: number, labelId: number): Promise<void> {
    await api.post(`/TaskItems/${taskId}/labels`, { taskId, labelId })
  },

  async removeLabel(taskId: number, labelId: number): Promise<void> {
    await api.delete(`/TaskItems/${taskId}/labels/${labelId}`)
  }
}
