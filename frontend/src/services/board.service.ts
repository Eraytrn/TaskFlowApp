import { api } from './api'
import type { Board, CreateBoardRequest, UpdateBoardRequest } from '@/types/board'

interface PaginatedResponse<T> {
  items: T[]
  pageNumber: number
  totalPages: number
  totalCount: number
  hasPreviousPage: boolean
  hasNextPage: boolean
}

export const boardService = {
  async getAll(): Promise<Board[]> {
    const response = await api.get<PaginatedResponse<Board>>('/Boards')
    return response.data.items
  },

  async getById(id: number): Promise<Board> {
    const response = await api.get<Board>(`/Boards/${id}`)
    return response.data
  },

  async create(data: CreateBoardRequest): Promise<Board> {
    const response = await api.post<Board>('/Boards', data)
    return response.data
  },

  async update(data: UpdateBoardRequest): Promise<void> {
    await api.put(`/Boards/${data.id}`, data)
  },

  async delete(id: number): Promise<void> {
    await api.delete(`/Boards/${id}`)
  }
}
