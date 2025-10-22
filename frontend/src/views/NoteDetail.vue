<script setup lang="ts">
import { onMounted, ref, watch } from 'vue';
import { getNote, updateNote, deleteNote } from '@/api/notes';
import type { Note } from '@/types/note';
import { useRoute, useRouter } from 'vue-router';
import { showSuccess, showError } from '@/lib/toast';

const route = useRoute();
const router = useRouter();
const id = ref<string>(route.params.id as string);
const loading = ref(false);
const error = ref<string | null>(null);
const note = ref<Note | null>(null);
const title = ref('');
const content = ref('');
const saving = ref(false);

async function load() {
    loading.value = true;
    error.value = null;
    try {
        const data = await getNote(id.value);
        note.value = data;
        title.value = data.title;
        content.value = data.content ?? '';
    } catch (e: any) {
        error.value = e?.message ?? 'Failed to load note';
    } finally {
        loading.value = false;
    }
}

async function save() {
    if (!note.value) return;
    if (!title.value.trim()) {
        showError('Title is required');
        return;
    }
    saving.value = true;
    try {
        const updated = await updateNote(id.value, { title: title.value.trim(), content: content.value.trim() || null });
        note.value = updated;
        showSuccess(`"${updated.title}" has been updated successfully`);
    } catch (e: any) {
        showError(e?.message ?? 'Failed to save note');
    } finally {
        saving.value = false;
    }
}

async function onDelete() {
    if (!confirm('Are you sure you want to delete this note? This action cannot be undone.')) return;

    const noteToDelete = note.value;
    try {
        await deleteNote(id.value);
        showSuccess(`"${noteToDelete?.title || 'Note'}" has been deleted successfully`);
        router.replace({ name: 'notes-list' });
    } catch (e: any) {
        showError(e?.message ?? 'Failed to delete note');
    }
}

watch(() => route.params.id, (val) => {
    id.value = String(val);
    load();
});

onMounted(load);
</script>

<template>
  <div class="space-y-8">
    <!-- Loading Skeleton -->
    <div v-if="loading" class="space-y-6">
      <div class="flex items-center justify-center gap-3 mb-8">
        <div class="w-8 h-8 border-2 border-blue-500 border-t-transparent rounded-full animate-spin"></div>
        <span class="text-gray-600 dark:text-gray-400 font-medium">Loading note...</span>
      </div>

      <div class="card border-0 shadow-xl animate-pulse">
        <div class="space-y-4">
          <div class="flex items-start justify-between">
            <div class="space-y-3 flex-1">
              <div class="h-8 w-3/4 bg-gray-200 dark:bg-gray-700 rounded-lg"></div>
              <div class="h-4 w-1/2 bg-gray-200 dark:bg-gray-700 rounded"></div>
            </div>
            <div class="w-20 h-8 bg-gray-200 dark:bg-gray-700 rounded-lg"></div>
          </div>
          <div class="grid grid-cols-1 sm:grid-cols-2 gap-3">
            <div class="h-4 w-full bg-gray-200 dark:bg-gray-700 rounded"></div>
            <div class="h-4 w-full bg-gray-200 dark:bg-gray-700 rounded"></div>
          </div>
        </div>
      </div>

      <div class="card border-0 shadow-xl animate-pulse">
        <div class="space-y-6">
          <div class="h-6 w-32 bg-gray-200 dark:bg-gray-700 rounded"></div>
          <div class="space-y-3">
            <div class="h-12 w-full bg-gray-200 dark:bg-gray-700 rounded-xl"></div>
            <div class="h-40 w-full bg-gray-200 dark:bg-gray-700 rounded-xl"></div>
          </div>
          <div class="flex gap-4 pt-6 border-t border-gray-200 dark:border-gray-700">
            <div class="h-11 w-32 bg-gray-200 dark:bg-gray-700 rounded-xl"></div>
            <div class="h-11 w-28 bg-gray-200 dark:bg-gray-700 rounded-xl"></div>
          </div>
        </div>
      </div>
    </div>

    <!-- Error State -->
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
          <h3 class="text-lg font-semibold text-red-900 dark:text-red-100">Unable to load note</h3>
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

    <!-- Note Content -->
    <div v-else-if="note" class="space-y-8">
      <!-- Header with metadata -->
      <div class="card border-0 shadow-xl">
        <div class="flex items-start justify-between mb-6">
          <div class="flex items-center gap-4">
            <div class="w-12 h-12 bg-gradient-to-br from-blue-500 to-indigo-600 rounded-2xl flex items-center justify-center">
              <svg class="w-6 h-6 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
              </svg>
            </div>
            <div>
              <h1 class="text-3xl font-bold text-gray-900 dark:text-white mb-1">{{ note.title }}</h1>
              <div class="text-sm text-gray-500 flex items-center gap-2">
                <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 7h.01M7 3h5c.512 0 1.024.195 1.414.586l7 7a2 2 0 010 2.828l-7 7a2 2 0 01-2.828 0l-7-7A1.994 1.994 0 013 12V7a4 4 0 014-4z" />
                </svg>
                Note ID: {{ note.id.slice(0, 8) }}...
              </div>
            </div>
          </div>
          <button @click="() => router.back()" class="btn btn-secondary px-6">
            <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
            </svg>
            Back
          </button>
        </div>

        <div class="grid grid-cols-1 sm:grid-cols-2 gap-4 p-4 bg-gray-50 dark:bg-gray-800/50 rounded-xl">
          <div class="flex items-center gap-3 text-sm">
            <div class="w-8 h-8 bg-green-100 dark:bg-green-900/30 rounded-lg flex items-center justify-center">
              <svg class="w-4 h-4 text-green-600 dark:text-green-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3a2 2 0 012-2h4a2 2 0 012 2v4m-6 9l6-6m0 0v6m0-6h-6" />
              </svg>
            </div>
            <div>
              <div class="font-medium text-gray-900 dark:text-white">Created</div>
              <div class="text-gray-600 dark:text-gray-400">{{ new Date(note.createdAt).toLocaleString() }}</div>
            </div>
          </div>

          <div class="flex items-center gap-3 text-sm">
            <div class="w-8 h-8 bg-blue-100 dark:bg-blue-900/30 rounded-lg flex items-center justify-center">
              <svg class="w-4 h-4 text-blue-600 dark:text-blue-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15" />
              </svg>
            </div>
            <div>
              <div class="font-medium text-gray-900 dark:text-white">Updated</div>
              <div class="text-gray-600 dark:text-gray-400">{{ new Date(note.updatedAt).toLocaleString() }}</div>
            </div>
          </div>
        </div>
      </div>

      <!-- Edit Form -->
      <div class="card border-0 shadow-xl">
        <div class="flex items-center gap-3 mb-8">
          <div class="w-10 h-10 bg-gradient-to-br from-purple-500 to-pink-600 rounded-xl flex items-center justify-center">
            <svg class="w-5 h-5 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
            </svg>
          </div>
          <div>
            <h2 class="text-xl font-semibold">Edit Note</h2>
            <p class="text-sm text-gray-600 dark:text-gray-400">Make changes to your note</p>
          </div>
        </div>

        <form @submit.prevent="save" class="space-y-8">
          <!-- Enhanced Title Field -->
          <div class="form-group">
            <label for="title" class="form-label">
              <div class="w-8 h-8 bg-gradient-to-br from-blue-500 to-indigo-600 rounded-lg flex items-center justify-center mr-2">
                <svg class="w-4 h-4 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 7h.01M7 3h5c.512 0 1.024.195 1.414.586l7 7a2 2 0 010 2.828l-7 7a2 2 0 01-2.828 0l-7-7A1.994 1.994 0 013 12V7a4 4 0 014-4z" />
                </svg>
              </div>
              Note Title <span class="text-red-500">*</span>
            </label>
            <div class="relative">
              <input
                id="title"
                v-model="title"
                type="text"
                placeholder="e.g., Meeting Notes, Project Ideas, Personal Thoughts..."
                class="form-input pl-12"
                :class="{
                  'border-red-500 focus:border-red-500 focus:ring-red-500': !title.trim()
                }"
                required
              />
              <div class="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 bg-gradient-to-br from-blue-500 to-indigo-600 rounded-full flex items-center justify-center">
                <svg class="w-2 h-2 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="3" d="M7 7h.01M7 3h5c.512 0 1.024.195 1.414.586l7 7a2 2 0 010 2.828l-7 7a2 2 0 01-2.828 0l-7-7A1.994 1.994 0 013 12V7a4 4 0 014-4z" />
                </svg>
              </div>
            </div>
            <div class="flex items-center justify-between mt-2">
              <p class="text-xs text-gray-500">Choose a clear, descriptive title for your note</p>
              <span class="text-xs text-gray-400">{{ title.length }}/100</span>
            </div>
          </div>

          <!-- Enhanced Content Field -->
          <div class="form-group">
            <label for="content" class="form-label">
              <div class="w-8 h-8 bg-gradient-to-br from-purple-500 to-pink-600 rounded-lg flex items-center justify-center mr-2">
                <svg class="w-4 h-4 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
                </svg>
              </div>
              Note Content
            </label>
            <div class="relative">
              <textarea
                id="content"
                v-model="content"
                rows="16"
                placeholder="Write your thoughts, ideas, meeting notes, or anything you want to remember...&#10;&#10;Tips:&#10;• Use markdown for formatting&#10;• Add bullet points with - or *&#10;• Create headings with #&#10;• Make lists with numbers 1. 2. 3."
                class="form-textarea pl-12"
                maxlength="10000"
              ></textarea>
              <div class="absolute left-4 top-4 w-4 h-4 bg-gradient-to-br from-purple-500 to-pink-600 rounded-full flex items-center justify-center">
                <svg class="w-2 h-2 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="3" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
                </svg>
              </div>
            </div>
            <div class="flex items-center justify-between mt-2">
              <p class="text-xs text-gray-500">Share your thoughts, ideas, and inspirations</p>
              <span class="text-xs text-gray-400">{{ content.length }}/10000</span>
            </div>
          </div>

          <!-- Actions -->
          <div class="flex flex-col sm:flex-row gap-4 pt-8 border-t border-gray-200/50 dark:border-gray-700/50">
            <button
              type="submit"
              :disabled="saving || !title.trim()"
              class="btn btn-primary flex-1 sm:flex-none px-8 py-3 text-base font-semibold"
            >
              <svg v-if="saving" class="animate-spin -ml-1 mr-3 h-5 w-5" fill="none" viewBox="0 0 24 24">
                <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
              </svg>
              <svg v-else class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" />
              </svg>
              {{ saving ? 'Saving Changes...' : '💾 Save Changes' }}
            </button>

            <button
              type="button"
              @click="onDelete"
              :disabled="saving"
              class="btn btn-danger flex-1 sm:flex-none px-8 py-3 text-base font-medium"
            >
              <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
              </svg>
              Delete Note
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>


