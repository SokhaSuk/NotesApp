<template>
  <div class="max-w-2xl mx-auto">
    <div class="flex items-center justify-between mb-6">
      <h1 class="text-3xl font-bold text-gray-900">Create New Note</h1>
      <router-link to="/notes" class="btn-secondary">
        Back to Notes
      </router-link>
    </div>

    <form @submit.prevent="handleSubmit" class="card space-y-6">
      <!-- Title Field -->
      <div>
        <label for="title" class="block text-sm font-medium text-gray-700 mb-2">
          Title *
        </label>
        <input
          id="title"
          v-model="form.title"
          type="text"
          required
          class="input-field"
          placeholder="Enter note title..."
          :disabled="loading"
        />
        <p v-if="errors.title" class="mt-1 text-sm text-red-600">
          {{ errors.title }}
        </p>
      </div>

      <!-- Content Field -->
      <div>
        <label for="content" class="block text-sm font-medium text-gray-700 mb-2">
          Content
        </label>
        <textarea
          id="content"
          v-model="form.content"
          rows="8"
          class="input-field resize-vertical"
          placeholder="Write your note content here..."
          :disabled="loading"
        ></textarea>
        <p v-if="errors.content" class="mt-1 text-sm text-red-600">
          {{ errors.content }}
        </p>
      </div>

      <!-- Error Display -->
      <div v-if="error" class="text-red-600 text-sm">
        {{ error }}
      </div>

      <!-- Form Actions -->
      <div class="flex gap-4">
        <button
          type="submit"
          class="btn-primary flex-1"
          :disabled="loading"
        >
          <LoadingSpinner v-if="loading" />
          <span v-else>Create Note</span>
        </button>

        <router-link
          to="/notes"
          class="btn-secondary flex-1 text-center"
          :disabled="loading"
        >
          Cancel
        </router-link>
      </div>
    </form>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { useNotesStore } from '@/stores/notes'
import LoadingSpinner from '@/components/LoadingSpinner.vue'

const router = useRouter()
const notesStore = useNotesStore()

const form = reactive({
  title: '',
  content: '',
})

const errors = ref<Record<string, string>>({})
const loading = ref(false)
const error = ref<string | null>(null)

function validateForm(): boolean {
  errors.value = {}

  if (!form.title.trim()) {
    errors.value.title = 'Title is required'
  }

  if (form.title.length > 200) {
    errors.value.title = 'Title must be less than 200 characters'
  }

  if (form.content && form.content.length > 5000) {
    errors.value.content = 'Content must be less than 5000 characters'
  }

  return Object.keys(errors.value).length === 0
}

async function handleSubmit() {
  if (!validateForm()) return

  loading.value = true
  error.value = null

  try {
    await notesStore.createNote({
      title: form.title.trim(),
      content: form.content.trim() || undefined,
    })

    router.push('/notes')
  } catch (err: any) {
    error.value = err.message
  } finally {
    loading.value = false
  }
}
</script>
