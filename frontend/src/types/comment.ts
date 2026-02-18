export interface Comment {
    id: number
    content: string
    taskItemId: number
    userId: number
    userName: string
    createdDate: string
}

export interface CreateCommentRequest {
    content: string
    taskItemId: number
    userId: number
}
