export const NotificationType = {
    JoinRequest: 0,
    RequestApproved: 1,
    RequestRejected: 2,
    TaskAssigned: 3
} as const

export type NotificationType = typeof NotificationType[keyof typeof NotificationType]

export interface Notification {
    id: number
    userId: number
    title: string
    message: string
    type: NotificationType
    relatedEntityId?: number
    isRead: boolean
    createdDate: string
}
