export interface LoginRequest {
  email: string
  password: string
}

export interface RegisterRequest {
  fullName: string
  email: string
  password: string
}

export interface AuthResponse {
  token: string
  userId: number
  email: string
  fullName: string
  expiresAt: string
}

export interface User {
  id: number
  fullName: string
  email: string
  createdDate: string
}
