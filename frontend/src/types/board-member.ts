export const BoardMemberRole = {
    Owner: 0,
    Member: 1
} as const

export type BoardMemberRole = typeof BoardMemberRole[keyof typeof BoardMemberRole]

export interface BoardMember {
    id: number
    boardId: number
    userId: number
    userName: string
    userEmail: string
    role: BoardMemberRole
    joinedDate: string
}
