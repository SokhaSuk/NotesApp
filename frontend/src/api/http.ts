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
        (config.headers as Record<string, string>)['X-User-Id'] = DEFAULT_USER_ID;
        return config;
    });
    return instance;
}

export const http = createHttpClient();


