import { httpClient } from './http'
import type { LoginRequest, RegisterRequest, AuthResponse, User } from '@/types/user'

export class AuthApi {
  static async login(credentials: LoginRequest): Promise<AuthResponse> {
    const response = await httpClient.post<AuthResponse>('/auth/login', credentials)
    return response.data
  }

  static async register(userData: RegisterRequest): Promise<AuthResponse> {
    const response = await httpClient.post<AuthResponse>('/auth/register', userData)
    return response.data
  }

  static async getCurrentUser(): Promise<User> {
    const response = await httpClient.get<User>('/auth/me')
    return response.data
  }

  static logout(): void {
    localStorage.removeItem('auth_token')
  }
}
