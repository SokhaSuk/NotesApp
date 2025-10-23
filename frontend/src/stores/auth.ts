import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { AuthApi } from '@/api/auth'
import type { User, LoginRequest, RegisterRequest } from '@/types/user'

export const useAuthStore = defineStore('auth', () => {
  const user = ref<User | null>(null)
  const token = ref<string | null>(localStorage.getItem('auth_token'))
  const loading = ref(false)
  const error = ref<string | null>(null)

  const isAuthenticated = computed(() => !!token.value)

  async function login(credentials: LoginRequest) {
    loading.value = true
    error.value = null

    try {
      const response = await AuthApi.login(credentials)
      token.value = response.token
      user.value = response.user
      localStorage.setItem('auth_token', response.token)
      return response
    } catch (err: any) {
      error.value = err.message
      throw err
    } finally {
      loading.value = false
    }
  }

  async function register(userData: RegisterRequest) {
    loading.value = true
    error.value = null

    try {
      const response = await AuthApi.register(userData)
      token.value = response.token
      user.value = response.user
      localStorage.setItem('auth_token', response.token)
      return response
    } catch (err: any) {
      error.value = err.message
      throw err
    } finally {
      loading.value = false
    }
  }

  async function loadCurrentUser() {
    if (!token.value) return

    loading.value = true
    error.value = null

    try {
      user.value = await AuthApi.getCurrentUser()
    } catch (err: any) {
      error.value = err.message
      logout()
    } finally {
      loading.value = false
    }
  }

  function logout() {
    token.value = null
    user.value = null
    localStorage.removeItem('auth_token')
    AuthApi.logout()
  }

  return {
    user,
    token,
    loading,
    error,
    isAuthenticated,
    login,
    register,
    loadCurrentUser,
    logout,
  }
})
