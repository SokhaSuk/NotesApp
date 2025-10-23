import axios, { AxiosInstance, AxiosResponse } from 'axios'
import type { ApiResponse, ApiError, BackendErrorResponse } from '@/types/api'

class HttpClient {
  private instance: AxiosInstance

  constructor() {
    this.instance = axios.create({
      baseURL: '/api',
      timeout: 10000,
      headers: {
        'Content-Type': 'application/json',
      },
    })

    this.setupInterceptors()
  }

  private setupInterceptors() {
    // Request interceptor to add auth token
    this.instance.interceptors.request.use(
      (config) => {
        const token = localStorage.getItem('auth_token')
        if (token) {
          config.headers.Authorization = `Bearer ${token}`
        }
        return config
      },
      (error) => {
        return Promise.reject(error)
      }
    )

    // Response interceptor to handle common errors
    this.instance.interceptors.response.use(
      (response) => response,
      (error) => {
        if (error.response?.status === 401) {
          localStorage.removeItem('auth_token')
          window.location.href = '/login'
        }
        return Promise.reject(error)
      }
    )
  }

  async request<T>(method: string, url: string, data?: any): Promise<ApiResponse<T>> {
    try {
      const response: AxiosResponse<any> = await this.instance.request({
        method,
        url,
        data,
      })

      // Transform backend response format to frontend format
      if (response.data.Data !== undefined) {
        return {
          data: response.data.Data,
          message: response.data.Message || '',
          success: response.data.Success !== false,
        }
      }

      // If already in frontend format, return as is
      return response.data
    } catch (error: any) {
      throw this.handleError(error)
    }
  }

  private handleError(error: any): ApiError {
    if (error.response?.data) {
      const data = error.response.data as BackendErrorResponse

      // Handle backend ErrorResponse format
      if (data.Message) {
        return {
          message: data.Message,
          errors: data.Errors,
        }
      }

    }
    return {
      message: error.message || 'Network error occurred',
    }
  }

  get<T>(url: string): Promise<ApiResponse<T>> {
    return this.request<T>('GET', url)
  }

  post<T>(url: string, data?: any): Promise<ApiResponse<T>> {
    return this.request<T>('POST', url, data)
  }

  put<T>(url: string, data?: any): Promise<ApiResponse<T>> {
    return this.request<T>('PUT', url, data)
  }

  delete<T>(url: string): Promise<ApiResponse<T>> {
    return this.request<T>('DELETE', url)
  }
}

export const httpClient = new HttpClient()
