export interface Note {
  id: number
  title: string
  content?: string
  createdAt: string
  updatedAt: string
  userId: number
}

export interface CreateNoteRequest {
  title: string
  content?: string
}

export interface UpdateNoteRequest {
  title: string
  content?: string
}

export interface NotesResponse {
  notes: Note[]
  totalCount: number
  page: number
  pageSize: number
}

export interface NoteFilters {
  search?: string
  sortBy?: 'title' | 'createdAt' | 'updatedAt'
  sortOrder?: 'asc' | 'desc'
  page?: number
  pageSize?: number
}
