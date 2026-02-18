import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { authService } from '@/services/auth.service'
import type { LoginRequest, RegisterRequest, User } from '@/types/auth'

export const useAuthStore = defineStore('auth', () => {
  const token = ref<string | null>(localStorage.getItem('token'))
  const user = ref<User | null>(null)
  const loading = ref(false)
  const error = ref<string | null>(null)

  const isAuthenticated = computed(() => !!token.value)

  async function login(credentials: LoginRequest) {
    try {
      loading.value = true
      error.value = null

      const response = await authService.login(credentials)

      token.value = response.token
      user.value = {
        id: response.userId,
        email: response.email,
        fullName: response.fullName,
        createdDate: new Date().toISOString()
      }

      localStorage.setItem('token', response.token)
      localStorage.setItem('user', JSON.stringify(user.value))

      return true
    } catch (err: any) {
      error.value = err.response?.data?.message || 'Login failed'
      return false
    } finally {
      loading.value = false
    }
  }

  async function register(data: RegisterRequest) {
    try {
      loading.value = true
      error.value = null

      console.log('Sending registration data:', data)
      const response = await authService.register(data)
      console.log('Registration response:', response)

      token.value = response.token
      user.value = {
        id: response.userId,
        email: response.email,
        fullName: response.fullName,
        createdDate: new Date().toISOString()
      }

      localStorage.setItem('token', response.token)
      localStorage.setItem('user', JSON.stringify(user.value))

      return true
    } catch (err: any) {
      console.error('Registration error:', err)
      console.error('Error response:', err.response)
      error.value = err.response?.data?.message || err.response?.data || err.message || 'Registration failed'
      return false
    } finally {
      loading.value = false
    }
  }

  function logout() {
    authService.logout()
    token.value = null
    user.value = null
  }

  function loadUserFromStorage() {
    const storedUser = localStorage.getItem('user')
    if (storedUser) {
      try {
        user.value = JSON.parse(storedUser)
      } catch {
        localStorage.removeItem('user')
      }
    }
  }

  return {
    token,
    user,
    loading,
    error,
    isAuthenticated,
    login,
    register,
    logout,
    loadUserFromStorage
  }
})
