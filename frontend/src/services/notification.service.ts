import { api } from './api'
import type { Notification } from '@/types/notification'

export const notificationService = {
    async getUnread(): Promise<Notification[]> {
        const response = await api.get<Notification[]>('/Notifications/unread')
        return response.data
    },

    async markAsRead(notificationId: number): Promise<void> {
        await api.patch(`/Notifications/${notificationId}/read`)
    }
}
