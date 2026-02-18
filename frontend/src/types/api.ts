export interface ApiError {
  message?: string
  errors?: string[]
}

export interface ApiResponse<T> {
  data?: T
  errors?: string[]
  isSuccess: boolean
}
