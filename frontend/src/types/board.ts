export interface Board {
  id: number
  title: string
  description?: string
  ownerId: number
  ownerName: string
  createdDate: string
  updatedDate: string
  labels?: import('./label').Label[]
}

export interface CreateBoardRequest {
  title: string
  description?: string
  ownerId: number
}

export interface UpdateBoardRequest {
  id: number
  title: string
  description?: string
}
