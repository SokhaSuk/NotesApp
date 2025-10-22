import axios, { type AxiosInstance } from 'axios';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5099/api';
const DEFAULT_USER_ID = import.meta.env.VITE_USER_ID || 'demo-user';

function createHttpClient(): AxiosInstance {
    const instance = axios.create({
        baseURL: API_BASE_URL,
        headers: {
            'Content-Type': 'application/json'
        }
    });
    instance.interceptors.request.use((config) => {
        config.headers = config.headers || {};
        config.headers['X-User-Id'] = DEFAULT_USER_ID;
        return config;
    });
    return instance;
}

const http = createHttpClient();

export type Note = {
    id: string;
    title: string;
    content?: string | null;
    createdAt: string;
    updatedAt: string;
};

export type NoteCreateRequest = {
    title: string;
    content?: string | null;
};

export type NoteUpdateRequest = {
    title: string;
    content?: string | null;
};

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


