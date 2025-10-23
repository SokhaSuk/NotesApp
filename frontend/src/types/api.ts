export interface ApiResponse<T> {
  data: T
  message?: string
  success: boolean
}

// Backend response format (what the server actually returns)
export interface BackendApiResponse<T> {
  Data: T
  Message?: string
  Success: boolean
}

// Backend error response format
export interface BackendErrorResponse {
  Message: string
  Errors?: Record<string, string[]>
}

export interface ApiError {
  message: string
  errors?: Record<string, string[]>
}

export type HttpMethod = 'GET' | 'POST' | 'PUT' | 'DELETE' | 'PATCH'
