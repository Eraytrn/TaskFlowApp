import { api } from './api'
import type { Comment, CreateCommentRequest } from '@/types/comment'

export const commentService = {
    async getByTaskId(taskItemId: number): Promise<Comment[]> {
        const response = await api.get<Comment[]>(`/Comments/task/${taskItemId}`)
        return response.data
    },

    async create(data: CreateCommentRequest): Promise<Comment> {
        const response = await api.post<Comment>('/Comments', data)
        return response.data
    },

    async delete(id: number): Promise<void> {
        await api.delete(`/Comments/${id}`)
    }
}
