<script setup lang="ts">
import { onMounted, ref, computed } from 'vue';
import { listNotes, deleteNote } from '@/api/notes';
import type { Note } from '@/types/note';
import { useRouter } from 'vue-router';
import { showSuccess, showError } from '@/lib/toast';

const router = useRouter();
const loading = ref(false);
const error = ref<string | null>(null);
const notes = ref<Array<Pick<Note, 'id' | 'title' | 'createdAt' | 'updatedAt'>>>([]);
const search = ref('');
const sortBy = ref<'createdAt' | 'updatedAt' | 'title'>('createdAt');
const sortDir = ref<'asc' | 'desc'>('desc');
const searchTimeout = ref<ReturnType<typeof setTimeout> | null>(null);

async function load() {
    loading.value = true;
    error.value = null;
    try {
        notes.value = await listNotes({ search: search.value || undefined, sortBy: sortBy.value, sortDir: sortDir.value });
    } catch (e: any) {
        const message = e?.message ?? 'Failed to load notes';
        error.value = message;
        showError(message);
    } finally {
        loading.value = false;
    }
}

function debouncedSearch() {
    if (searchTimeout.value) {
        clearTimeout(searchTimeout.value);
    }
    searchTimeout.value = setTimeout(() => {
        load();
    }, 300);
}

function toCreate() {
    router.push({ name: 'note-create' });
}

function toDetail(id: string) {
    router.push({ name: 'note-detail', params: { id } });
}

async function onDelete(id: string) {
    if (!confirm('Are you sure you want to delete this note? This action cannot be undone.')) return;

    const noteToDelete = notes.value.find(n => n.id === id);
    try {
        await deleteNote(id);
        notes.value = notes.value.filter(n => n.id !== id);
        showSuccess(`"${noteToDelete?.title || 'Note'}" has been deleted successfully`);
    } catch (e: any) {
        showError(e?.message ?? 'Failed to delete note');
    }
}

function formatDate(dateString: string) {
    const date = new Date(dateString);
    const now = new Date();
    const diffTime = Math.abs(now.getTime() - date.getTime());
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));

    if (diffDays === 1) {
        return date.toLocaleDateString() + ' (Today)';
    } else if (diffDays === 2) {
        return date.toLocaleDateString() + ' (Yesterday)';
    } else if (diffDays <= 7) {
        return date.toLocaleDateString() + ' (' + diffDays + ' days ago)';
    } else {
        return date.toLocaleDateString();
    }
}

onMounted(() => {
    load();
});

const hasNotes = computed(() => notes.value.length > 0);
</script>

<template>
  <div class="space-y-8">
    <!-- Header -->
    <div class="text-center space-y-4">
      <div class="flex items-center justify-center gap-3 mb-6">
        <div class="relative">
          <div class="w-16 h-16 bg-gradient-to-br from-blue-500 to-indigo-600 rounded-2xl flex items-center justify-center shadow-lg animate-pulse-glow">
            <svg class="w-8 h-8 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 7h.01M7 3h5c.512 0 1.024.195 1.414.586l7 7a2 2 0 010 2.828l-7 7a2 2 0 01-2.828 0l-7-7A1.994 1.994 0 013 12V7a4 4 0 014-4z" />
            </svg>
          </div>
          <div class="absolute -top-1 -right-1 w-6 h-6 bg-gradient-to-br from-emerald-400 to-green-500 rounded-full flex items-center justify-center animate-float">
            <svg class="w-3 h-3 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="3" d="M5 13l4 4L19 7" />
            </svg>
          </div>
        </div>
        <div class="text-left">
          <h1 class="text-4xl font-bold bg-gradient-to-r from-gray-900 to-gray-600 dark:from-white dark:to-gray-300 bg-clip-text text-transparent">Your Notes</h1>
          <p class="text-gray-600 dark:text-gray-400 text-lg">Organize your thoughts beautifully</p>
        </div>
      </div>

      <div class="flex flex-col sm:flex-row items-center justify-center gap-3">
        <button @click="load" class="btn btn-secondary" :disabled="loading">
          <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15" />
          </svg>
          {{ loading ? 'Refreshing...' : 'Refresh' }}
        </button>
        <button @click="toCreate" class="btn btn-primary">
          <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
          </svg>
          Create Note
        </button>
      </div>
    </div>

    <!-- Search and filters -->
    <div class="card border-0 shadow-xl">
      <div class="flex items-center gap-3 mb-6">
        <div class="w-10 h-10 bg-gradient-to-br from-blue-500 to-indigo-600 rounded-xl flex items-center justify-center">
          <svg class="w-5 h-5 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 4a1 1 0 011-1h16a1 1 0 011 1v2.586a1 1 0 01-.293.707l-6.414 6.414a1 1 0 00-.293.707V17l-4 4v-6.586a1 1 0 00-.293-.707L3.293 7.707A1 1 0 013 7V4z" />
          </svg>
        </div>
        <div>
          <h2 class="text-lg font-semibold">Search & Filter</h2>
          <p class="text-sm text-gray-600 dark:text-gray-400">Find and organize your notes</p>
        </div>
      </div>

      <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
        <div class="form-group">
          <label class="form-label">
            <svg class="w-4 h-4 text-blue-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
            </svg>
            Search Notes
          </label>
          <div class="relative">
            <input
              v-model="search"
              @input="debouncedSearch"
              placeholder="Type to search by title..."
              class="form-input pl-12"
            />
            <svg class="w-5 h-5 text-gray-400 absolute left-4 top-1/2 -translate-y-1/2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
            </svg>
          </div>
        </div>

        <div class="form-group">
          <label class="form-label">
            <svg class="w-4 h-4 text-emerald-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 16V4m0 0L3 8m4-4l4 4m6 0v12m0 0l4-4m-4 4l-4-4" />
            </svg>
            Sort By
          </label>
          <select v-model="sortBy" @change="load" class="form-input">
            <option value="createdAt">📅 Created Date</option>
            <option value="updatedAt">🔄 Updated Date</option>
            <option value="title">🔤 Title</option>
          </select>
        </div>

        <div class="form-group">
          <label class="form-label">
            <svg class="w-4 h-4 text-purple-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 11l5-5m0 0l5 5m-5-5v12" />
            </svg>
            Order
          </label>
          <select v-model="sortDir" @change="load" class="form-input">
            <option value="desc">🆕 Newest First</option>
            <option value="asc">📜 Oldest First</option>
          </select>
        </div>
      </div>

      <div class="flex items-center justify-between mt-6 pt-6 border-t border-gray-200/50 dark:border-gray-700/50" v-if="hasNotes">
        <div class="flex items-center gap-2 text-sm text-gray-600 dark:text-gray-400">
          <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
          </svg>
          <span class="font-medium">{{ notes.length }}</span>
          <span>{{ notes.length === 1 ? 'note found' : 'notes found' }}</span>
          <span v-if="search.trim()">for "{{ search }}"</span>
        </div>

        <div class="flex items-center gap-2">
          <div class="w-2 h-2 bg-green-500 rounded-full animate-pulse"></div>
          <span class="text-xs text-gray-500">Live search</span>
        </div>
      </div>
    </div>

    <!-- Loading -->
    <div v-if="loading" class="space-y-4">
      <div class="flex items-center justify-center gap-3 mb-8">
        <div class="w-8 h-8 border-2 border-blue-500 border-t-transparent rounded-full animate-spin"></div>
        <span class="text-gray-600 dark:text-gray-400 font-medium">Loading your notes...</span>
      </div>

      <div class="grid gap-6 md:grid-cols-2 xl:grid-cols-3">
        <div v-for="i in 6" :key="i" class="note-card animate-pulse">
          <div class="space-y-4">
            <div class="flex items-start justify-between">
              <div class="flex-1 space-y-2">
                <div class="h-6 w-3/4 bg-gray-200 dark:bg-gray-700 rounded-lg"></div>
                <div class="flex items-center gap-2">
                  <div class="h-3 w-20 bg-gray-200 dark:bg-gray-700 rounded"></div>
                  <div class="h-3 w-16 bg-gray-200 dark:bg-gray-700 rounded"></div>
                </div>
              </div>
              <div class="w-8 h-8 bg-gray-200 dark:bg-gray-700 rounded-lg"></div>
            </div>
            <div class="space-y-2">
              <div class="h-4 w-full bg-gray-200 dark:bg-gray-700 rounded"></div>
              <div class="h-4 w-2/3 bg-gray-200 dark:bg-gray-700 rounded"></div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Error -->
    <div v-else-if="error" class="card border-red-200/50 bg-gradient-to-br from-red-50 to-rose-50 dark:from-red-900/20 dark:to-rose-900/20 dark:border-red-800/50">
      <div class="flex items-center gap-4">
        <div class="flex-shrink-0">
          <div class="w-12 h-12 bg-gradient-to-br from-red-500 to-rose-600 rounded-xl flex items-center justify-center">
            <svg class="w-6 h-6 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
            </svg>
          </div>
        </div>
        <div class="flex-1">
          <h3 class="text-lg font-semibold text-red-900 dark:text-red-100">Oops! Something went wrong</h3>
          <p class="text-red-700 dark:text-red-300 mt-1">{{ error }}</p>
        </div>
        <button @click="load" class="btn btn-secondary">
          <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15" />
          </svg>
          Try Again
        </button>
      </div>
    </div>

    <!-- Notes list -->
    <div v-else-if="hasNotes" class="space-y-6">
      <div class="grid gap-6 md:grid-cols-2 xl:grid-cols-3">
        <div
          v-for="note in notes"
          :key="note.id"
          @click="toDetail(note.id)"
          class="note-card group cursor-pointer"
        >
          <div class="space-y-4">
            <!-- Note Header -->
            <div class="flex items-start justify-between">
              <div class="flex-1 min-w-0">
                <div class="flex items-center gap-2 mb-2">
                  <div class="w-2 h-2 bg-gradient-to-r from-blue-500 to-indigo-600 rounded-full"></div>
                  <h3 class="text-lg font-semibold text-gray-900 dark:text-white transition-colors duration-200 truncate">
                    {{ note.title }}
                  </h3>
                </div>
                <div class="flex items-center gap-3 text-xs text-gray-500 dark:text-gray-400">
                  <span class="inline-flex items-center gap-1 bg-green-50 dark:bg-green-900/20 text-green-700 dark:text-green-300 px-2 py-1 rounded-full">
                    <svg class="w-3 h-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3a2 2 0 012-2h4a2 2 0 012 2v4m-6 9l6-6m0 0v6m0-6h-6" />
                    </svg>
                    {{ formatDate(note.createdAt) }}
                  </span>
                  <span class="inline-flex items-center gap-1 bg-blue-50 dark:bg-blue-900/20 text-blue-700 dark:text-blue-300 px-2 py-1 rounded-full">
                    <svg class="w-3 h-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15" />
                    </svg>
                    {{ formatDate(note.updatedAt) }}
                  </span>
                </div>
              </div>

              <!-- Delete Button -->
              <div class="note-actions ml-3">
                <button
                  @click.stop="onDelete(note.id)"
                  class="btn btn-danger p-2 hover:scale-110 transition-transform"
                  title="Delete note"
                >
                  <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                  </svg>
                </button>
              </div>
            </div>

            <!-- Note Preview/Content -->
            <div class="text-sm text-gray-600 dark:text-gray-400 line-clamp-3 leading-relaxed">
              📝 Click to view and edit details
            </div>

            <!-- Note Footer -->
            <div class="flex items-center justify-between pt-3 border-t border-gray-200/50 dark:border-gray-700/50">
              <div class="flex items-center gap-2 text-xs text-gray-500 dark:text-gray-400">
                <svg class="w-3 h-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
                </svg>
                <span>Note</span>
              </div>

              <div class="flex items-center gap-1 text-xs text-gray-400">
                <svg class="w-3 h-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
                </svg>
                <span>Click to view</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Empty state -->
    <div v-else class="text-center py-16">
      <div class="max-w-md mx-auto">
        <div class="w-24 h-24 bg-gradient-to-br from-gray-100 to-gray-200 dark:from-gray-800 dark:to-gray-700 rounded-3xl flex items-center justify-center mx-auto mb-6 animate-float">
          <svg class="w-12 h-12 text-gray-400 dark:text-gray-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
          </svg>
        </div>

        <h3 class="text-2xl font-bold text-gray-900 dark:text-white mb-3">No notes yet</h3>
        <p class="text-gray-600 dark:text-gray-400 mb-8 text-lg leading-relaxed">
          Start your journey by creating your first note. Capture your thoughts, ideas, and inspirations in one beautiful place.
        </p>

        <div class="space-y-4">
          <button @click="toCreate" class="btn btn-primary px-8 py-3 text-base">
            <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
            </svg>
            Create Your First Note
          </button>

          <div class="flex items-center justify-center gap-6 text-sm text-gray-500 dark:text-gray-400">
            <div class="flex items-center gap-2">
              <div class="w-2 h-2 bg-green-500 rounded-full"></div>
              <span>Free to use</span>
            </div>
            <div class="flex items-center gap-2">
              <div class="w-2 h-2 bg-blue-500 rounded-full"></div>
              <span>Organize easily</span>
            </div>
            <div class="flex items-center gap-2">
              <div class="w-2 h-2 bg-purple-500 rounded-full"></div>
              <span>Search quickly</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>


