import { api } from './api'
import type { BoardJoinRequest, CreateJoinRequestDto } from '@/types/join-request'

export const joinRequestService = {
    async getMyRequests(): Promise<BoardJoinRequest[]> {
        const response = await api.get<BoardJoinRequest[]>('/BoardJoinRequests/my-requests')
        return response.data
    },

    async getPendingRequests(boardId: number): Promise<BoardJoinRequest[]> {
        const response = await api.get<BoardJoinRequest[]>(`/BoardJoinRequests/board/${boardId}`)
        return response.data
    },

    async createRequest(dto: CreateJoinRequestDto): Promise<BoardJoinRequest> {
        const response = await api.post<BoardJoinRequest>('/BoardJoinRequests', dto)
        return response.data
    },

    async approveRequest(requestId: number): Promise<void> {
        await api.patch(`/BoardJoinRequests/${requestId}/approve`)
    },

    async rejectRequest(requestId: number): Promise<void> {
        await api.patch(`/BoardJoinRequests/${requestId}/reject`)
    }
}
