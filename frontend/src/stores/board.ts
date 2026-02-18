import { defineStore } from 'pinia'
import { ref } from 'vue'
import { boardService } from '@/services/board.service'
import type { Board, CreateBoardRequest, UpdateBoardRequest } from '@/types/board'

export const useBoardStore = defineStore('board', () => {
  const boards = ref<Board[]>([])
  const currentBoard = ref<Board | null>(null)
  const loading = ref(false)
  const error = ref<string | null>(null)

  async function fetchBoards() {
    try {
      loading.value = true
      error.value = null
      boards.value = await boardService.getAll()
    } catch (err: any) {
      error.value = err.response?.data?.message || 'Failed to fetch boards'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function fetchBoard(id: number) {
    try {
      loading.value = true
      error.value = null
      currentBoard.value = await boardService.getById(id)
    } catch (err: any) {
      error.value = err.response?.data?.message || 'Failed to fetch board'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function createBoard(data: CreateBoardRequest) {
    try {
      loading.value = true
      error.value = null
      const newBoard = await boardService.create(data)
      boards.value.push(newBoard)
      return newBoard
    } catch (err: any) {
      error.value = err.response?.data?.message || 'Failed to create board'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function updateBoard(data: UpdateBoardRequest) {
    try {
      loading.value = true
      error.value = null
      await boardService.update(data)

      const board = boards.value.find(b => b.id === data.id)
      if (board) {
        board.title = data.title
        board.description = data.description
      }

      if (currentBoard.value?.id === data.id) {
        currentBoard.value.title = data.title
        currentBoard.value.description = data.description
      }
    } catch (err: any) {
      error.value = err.response?.data?.message || 'Failed to update board'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function deleteBoard(id: number) {
    try {
      loading.value = true
      error.value = null
      await boardService.delete(id)
      boards.value = boards.value.filter(b => b.id !== id)
      if (currentBoard.value?.id === id) {
        currentBoard.value = null
      }
    } catch (err: any) {
      error.value = err.response?.data?.message || 'Failed to delete board'
      throw err
    } finally {
      loading.value = false
    }
  }

  return {
    boards,
    currentBoard,
    loading,
    error,
    fetchBoards,
    fetchBoard,
    createBoard,
    updateBoard,
    deleteBoard
  }
})
