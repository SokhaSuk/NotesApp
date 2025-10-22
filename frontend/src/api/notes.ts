import { http } from '@/api/http';
import type { Note, NoteCreateRequest, NoteUpdateRequest } from '@/types/note';

export async function listNotes(params?: { search?: string; sortBy?: 'title' | 'createdAt' | 'updatedAt'; sortDir?: 'asc' | 'desc'; }): Promise<Pick<Note, 'id' | 'title' | 'createdAt' | 'updatedAt'>[]> {
    const { data } = await http.get('/notes', { params });
    return data;
}

export async function getNote(id: string): Promise<Note> {
    const { data } = await http.get(`/notes/${id}`);
    return data;
}

export async function createNote(payload: NoteCreateRequest): Promise<Note> {
    const { data } = await http.post('/notes', payload);
    return data;
}

export async function updateNote(id: string, payload: NoteUpdateRequest): Promise<Note> {
    const { data } = await http.put(`/notes/${id}`, payload);
    return data;
} 

export async function deleteNote(id: string): Promise<void> {
    await http.delete(`/notes/${id}`);
}