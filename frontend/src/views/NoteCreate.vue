<script setup lang="ts">
import { ref } from 'vue';
import { createNote } from '@/api/notes';
import { useRouter } from 'vue-router';
import { showSuccess, showError } from '@/lib/toast';

const router = useRouter();
const title = ref('');
const content = ref('');
const error = ref<string | null>(null);
const submitting = ref(false);

async function submit() {
    error.value = null;
    if (!title.value.trim()) {
        error.value = 'Title is required';
        showError('Please enter a title for your note');
        return;
    }
    submitting.value = true;
    try {
        const created = await createNote({ title: title.value.trim(), content: content.value.trim() || null });
        showSuccess(`"${created.title}" has been created successfully!`);
        router.replace({ name: 'note-detail', params: { id: created.id } });
    } catch (e: any) {
        const message = e?.message ?? 'Failed to create note';
        error.value = message;
        showError(message);
    } finally {
        submitting.value = false;
    }
}

function cancel() {
    router.back();
}
</script>

<template>
  <div class="space-y-8">
    <!-- Header -->
    <div class="text-center space-y-4">
      <div class="flex items-center justify-center gap-3 mb-6">
        <div class="relative">
          <div class="w-16 h-16 bg-gradient-to-br from-emerald-500 to-green-600 rounded-2xl flex items-center justify-center shadow-lg animate-pulse-glow">
            <svg class="w-8 h-8 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
            </svg>
          </div>
          <div class="absolute -top-1 -right-1 w-6 h-6 bg-gradient-to-br from-emerald-400 to-green-500 rounded-full flex items-center justify-center animate-float">
            <svg class="w-3 h-3 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="3" d="M5 13l4 4L19 7" />
            </svg>
          </div>
        </div>
        <div class="text-left">
          <h1 class="text-4xl font-bold bg-gradient-to-r from-gray-900 to-gray-600 dark:from-white dark:to-gray-300 bg-clip-text text-transparent">Create Note</h1>
          <p class="text-gray-600 dark:text-gray-400 text-lg">Write down your thoughts and ideas</p>
        </div>
      </div>
    </div>

    <!-- Error Alert -->
    <div v-if="error" class="card border-red-200/50 bg-gradient-to-br from-red-50 to-rose-50 dark:from-red-900/20 dark:to-rose-900/20 dark:border-red-800/50 animate-shake">
      <div class="flex items-center gap-4">
        <div class="flex-shrink-0">
          <div class="w-10 h-10 bg-gradient-to-br from-red-500 to-rose-600 rounded-xl flex items-center justify-center">
            <svg class="w-5 h-5 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
            </svg>
          </div>
        </div>
        <div class="flex-1">
          <h3 class="text-lg font-semibold text-red-900 dark:text-red-100">Oops! Something went wrong</h3>
          <p class="text-red-700 dark:text-red-300 mt-1">{{ error }}</p>
        </div>
        <button @click="error = null" class="btn btn-ghost p-2 hover:bg-red-100 dark:hover:bg-red-900/30">
          <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
          </svg>
        </button>
      </div>
    </div>

    <!-- Form -->
    <div class="card border-0 shadow-xl">
      <div class="flex items-center gap-3 mb-8">
        <div class="w-10 h-10 bg-gradient-to-br from-blue-500 to-indigo-600 rounded-xl flex items-center justify-center">
          <svg class="w-5 h-5 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
          </svg>
        </div>
        <div>
          <h2 class="text-xl font-semibold">Note Details</h2>
          <p class="text-sm text-gray-600 dark:text-gray-400">Fill in the information for your new note</p>
        </div>
      </div>

      <form @submit.prevent="submit" class="space-y-8">
        <!-- Title Field -->
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
                'border-red-500 focus:border-red-500 focus:ring-red-500': !title.trim() && error
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

        <!-- Content Field -->
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
              rows="12"
              placeholder="Write your thoughts, ideas, meeting notes, or anything you want to remember...&#10;&#10;Tips:&#10;• Use markdown for formatting&#10;• Add bullet points with - or *&#10;• Create headings with #&#10;• Make lists with numbers 1. 2. 3."
              class="form-textarea pl-12"
              maxlength="5000"
            ></textarea>
            <div class="absolute left-4 top-4 w-4 h-4 bg-gradient-to-br from-purple-500 to-pink-600 rounded-full flex items-center justify-center">
              <svg class="w-2 h-2 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="3" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
              </svg>
            </div>
          </div>
          <div class="flex items-center justify-between mt-2">
            <p class="text-xs text-gray-500">Share your thoughts, ideas, and inspirations</p>
            <span class="text-xs text-gray-400">{{ content.length }}/5000</span>
          </div>
        </div>

        <!-- Action Buttons -->
        <div class="flex flex-col sm:flex-row gap-4 pt-8 border-t border-gray-200/50 dark:border-gray-700/50">
          <button
            type="submit"
            :disabled="submitting || !title.trim()"
            class="btn btn-primary flex-1 sm:flex-none px-8 py-3 text-base font-semibold"
          >
            <svg v-if="submitting" class="animate-spin -ml-1 mr-3 h-5 w-5" fill="none" viewBox="0 0 24 24">
              <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
              <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
            </svg>
            <svg v-else class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
            </svg>
            {{ submitting ? 'Creating Your Note...' : '✨ Create Note' }}
          </button>

          <button
            type="button"
            @click="cancel"
            :disabled="submitting"
            class="btn btn-secondary px-8 py-3 text-base font-medium"
          >
            <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
            </svg>
            Cancel
          </button>
        </div>
      </form>
    </div>
    
  </div>
</template>


