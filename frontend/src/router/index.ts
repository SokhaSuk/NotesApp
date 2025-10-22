import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router';

const routes: RouteRecordRaw[] = [
    { path: '/', name: 'notes-list', component: () => import('../views/NotesList.vue') },
    { path: '/notes/new', name: 'note-create', component: () => import('../views/NoteCreate.vue') },
    { path: '/notes/:id', name: 'note-detail', component: () => import('../views/NoteDetail.vue'), props: true },
];

export const router = createRouter({
    history: createWebHistory(),
    routes,
});


