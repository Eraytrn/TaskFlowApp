import { api } from './api'
import type { BoardMember } from '@/types/board-member'
import type { Board } from '@/types/board'

export const boardMemberService = {
    async getBoardMembers(boardId: number): Promise<BoardMember[]> {
        const response = await api.get<BoardMember[]>(`/BoardMembers/board/${boardId}`)
        return response.data
    },

    async getMyBoards(): Promise<Board[]> {
        const response = await api.get<Board[]>('/BoardMembers/my-boards')
        return response.data
    },

    async addMember(boardId: number, userId: number): Promise<void> {
        await api.post('/BoardMembers', { boardId, userId })
    },

    async removeMember(boardId: number, userId: number): Promise<void> {
        await api.delete(`/BoardMembers/${boardId}/user/${userId}`)
    }
}
