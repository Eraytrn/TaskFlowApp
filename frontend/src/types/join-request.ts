export const RequestStatus = {
    Pending: 0,
    Approved: 1,
    Rejected: 2
} as const

export type RequestStatus = typeof RequestStatus[keyof typeof RequestStatus]

export interface BoardJoinRequest {
    id: number
    boardId: number
    boardTitle: string
    requesterId: number
    requesterName: string
    requesterEmail: string
    status: RequestStatus
    requestDate: string
    responseDate?: string
    responderId?: number
    responderName?: string
}

export interface CreateJoinRequestDto {
    boardId: number
    requesterId: number
}
