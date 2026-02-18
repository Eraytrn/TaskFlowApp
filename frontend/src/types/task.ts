export const TaskStatus = {
  Todo: 0,
  InProgress: 1,
  Done: 2
} as const

export type TaskStatus = typeof TaskStatus[keyof typeof TaskStatus]

export interface TaskItem {
  id: number
  title: string
  description: string
  status: TaskStatus
  statusText: string
  boardId: number
  boardName: string
  assignedUserId?: number
  assignedUserName?: string
  createdDate: string
  updatedDate: string

  labels?: import('./label').Label[]
}

export interface CreateTaskRequest {
  title: string
  description: string
  boardId: number
  assignedUserId?: number
}

export interface UpdateTaskRequest {
  id: number
  title: string
  description: string
  assignedUserId?: number

}
