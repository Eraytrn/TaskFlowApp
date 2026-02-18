import { api } from './api'
import type { Label } from '@/types/label'

export const labelService = {
    async create(boardId: number, name: string, colorHex: string): Promise<Label> {
        const response = await api.post<number>('/Labels', { boardId, name, colorHex })
        return { id: response.data, name, colorHex }
    },

    async delete(id: number): Promise<void> {
        await api.delete(`/Labels/${id}`)
    }
}
