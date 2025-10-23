import { httpClient } from './http'
import type { Note, CreateNoteRequest, UpdateNoteRequest, NotesResponse, NoteFilters } from '@/types/note'

export class NotesApi {
  static async getNotes(filters?: NoteFilters): Promise<NotesResponse> {
    const params = new URLSearchParams()

    if (filters?.search) params.append('search', filters.search)
    if (filters?.sortBy) params.append('sortBy', filters.sortBy)
    if (filters?.sortOrder) params.append('sortOrder', filters.sortOrder)
    if (filters?.page) params.append('page', filters.page.toString())
    if (filters?.pageSize) params.append('pageSize', filters.pageSize.toString())

    const query = params.toString()
    const response = await httpClient.get<NotesResponse>(`/notes${query ? `?${query}` : ''}`)
    return response.data
  }

  static async getNote(id: number): Promise<Note> {
    const response = await httpClient.get<Note>(`/notes/${id}`)
    return response.data
  }

  static async createNote(note: CreateNoteRequest): Promise<Note> {
    const response = await httpClient.post<Note>('/notes', note)
    return response.data
  }

  static async updateNote(id: number, note: UpdateNoteRequest): Promise<Note> {
    const response = await httpClient.put<Note>(`/notes/${id}`, note)
    return response.data
  }

  static async deleteNote(id: number): Promise<void> {
    await httpClient.delete(`/notes/${id}`)
  }
}
