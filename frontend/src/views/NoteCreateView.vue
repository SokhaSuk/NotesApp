<template>
  <div class="max-w-4xl mx-auto">
    <div class="text-center mb-8">
      <div class="inline-flex items-center justify-center w-20 h-20 bg-gradient-to-r from-blue-600 to-blue-800 rounded-3xl mb-6 shadow-xl">
      </div>
      <h1 class="text-4xl font-bold text-gradient mb-4">
        Create New Note
      </h1>
      <p class="text-gray-600 text-lg max-w-2xl mx-auto">
        Capture your thoughts, ideas, and inspirations in a beautiful note
      </p>
      <router-link
        to="/notes"
        class="btn-ghost mt-4 inline-flex items-center"
      >
        Back to Notes
      </router-link>
    </div>

    <form @submit.prevent="handleSubmit" class="card space-y-8">
      <!-- Title Field -->
      <div>
        <label for="title" class="block text-lg font-semibold text-gray-700 mb-3">
          Note Title *
        </label>
        <input
          id="title"
          v-model="form.title"
          type="text"
          required
          class="input-field text-lg"
          placeholder="What's your note about?"
          :disabled="loading"
        />
        <p v-if="errors.title" class="mt-2 text-sm text-red-600">
          {{ errors.title }}
        </p>
      </div>

      <!-- Content Field -->
      <div>
        <label for="content" class="block text-lg font-semibold text-gray-700 mb-3">
          Note Content
        </label>
        <textarea
          id="content"
          v-model="form.content"
          rows="12"
          class="input-field resize-vertical text-lg leading-relaxed"
          placeholder="Start writing your thoughts here... Let your creativity flow!"
          :disabled="loading"
        ></textarea>
        <p v-if="errors.content" class="mt-2 text-sm text-red-600">
          {{ errors.content }}
        </p>
      </div>

      <!-- Error Display -->
      <div v-if="error" class="bg-red-50 border border-red-200 rounded-xl p-6">
        <div class="flex items-center">
          <div>
            <h3 class="text-lg font-semibold text-red-800 mb-1">Something went wrong!</h3>
            <p class="text-red-700">{{ error }}</p>
          </div>
        </div>
      </div>

      <!-- Form Actions -->
      <div class="flex flex-col sm:flex-row gap-4 pt-6 border-t border-gray-200">
        <button
          type="submit"
          class="btn-primary flex-1 order-2 sm:order-1"
          :disabled="loading"
        >
          <LoadingSpinner v-if="loading" class="mx-auto" />
          <span v-else class="flex items-center justify-center">
            Create Note
          </span>
        </button>

        <router-link
          to="/notes"
          class="btn-ghost flex-1 order-1 sm:order-2 text-center"
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
