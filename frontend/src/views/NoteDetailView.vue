<template>
  <div class="max-w-4xl mx-auto">
    <!-- Loading State -->
    <div v-if="loading" class="flex justify-center py-8">
      <LoadingSpinner />
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="card">
      <div class="text-red-600 text-center">
        <p>{{ error }}</p>
        <button @click="fetchNote" class="btn-secondary mt-2">
          Try Again
        </button>
      </div>
    </div>

    <!-- Note Content -->
    <div v-else-if="note" class="space-y-6">
      <!-- Header -->
      <div class="flex items-center justify-between">
        <div v-if="!isEditing">
          <h1 class="text-3xl font-bold text-gray-900">{{ note.title }}</h1>
          <div class="text-sm text-gray-500 mt-1">
            <span>Created: {{ formatDate(note.createdAt) }}</span>
            <span v-if="note.updatedAt !== note.createdAt" class="ml-4">
              Updated: {{ formatDate(note.updatedAt) }}
            </span>
          </div>
        </div>

        <div class="flex gap-2">
          <button
            v-if="!isEditing"
            @click="startEditing"
            class="btn-primary"
          >
            Edit
          </button>

          <button
            v-if="isEditing"
            @click="cancelEditing"
            class="btn-secondary"
          >
            Cancel
          </button>

          <button
            @click="deleteNote"
            class="btn-danger"
          >
            Delete
          </button>
        </div>
      </div>

      <!-- Edit Form -->
      <form v-if="isEditing" @submit.prevent="handleUpdate" class="card space-y-6">
        <div>
          <label for="edit-title" class="block text-sm font-medium text-gray-700 mb-2">
            Title *
          </label>
          <input
            id="edit-title"
            v-model="editForm.title"
            type="text"
            required
            class="input-field"
          />
        </div>

        <div>
          <label for="edit-content" class="block text-sm font-medium text-gray-700 mb-2">
            Content
          </label>
          <textarea
            id="edit-content"
            v-model="editForm.content"
            rows="12"
            class="input-field resize-vertical"
          ></textarea>
        </div>

        <div class="flex gap-4">
          <button
            type="submit"
            class="btn-primary"
            :disabled="updateLoading"
          >
            <LoadingSpinner v-if="updateLoading" />
            <span v-else>Save Changes</span>
          </button>

          <button
            type="button"
            @click="cancelEditing"
            class="btn-secondary"
          >
            Cancel
          </button>
        </div>
      </form>

      <!-- View Mode -->
      <div v-else class="card">
        <div class="prose prose-gray max-w-none">
          <div v-if="note.content" class="whitespace-pre-wrap text-gray-700">
            {{ note.content }}
          </div>
          <div v-else class="text-gray-500 italic">
            No content
          </div>
        </div>
      </div>

      <!-- Navigation -->
      <div class="flex justify-between">
        <router-link to="/notes" class="btn-secondary">
          Back to Notes
        </router-link>
      </div>
    </div>

    <!-- Not Found -->
    <div v-else class="card">
      <div class="text-center py-12">
        <h3 class="text-lg font-medium text-gray-900 mb-2">Note not found</h3>
        <p class="text-gray-500 mb-4">The note you're looking for doesn't exist.</p>
        <router-link to="/notes" class="btn-primary">
          Back to Notes
        </router-link>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, computed, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useNotesStore } from '@/stores/notes'
import LoadingSpinner from '@/components/LoadingSpinner.vue'

const route = useRoute()
const router = useRouter()
const notesStore = useNotesStore()

const noteId = computed(() => parseInt(route.params.id as string))
const isEditing = ref(false)
const updateLoading = ref(false)

const { currentNote: note, loading, error, fetchNote, updateNote, deleteNote: deleteNoteAction } = notesStore

const editForm = reactive({
  title: '',
  content: '',
})

function formatDate(dateString: string): string {
  return new Date(dateString).toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  })
}

function startEditing() {
  if (note.value) {
    editForm.title = note.value.title
    editForm.content = note.value.content || ''
    isEditing.value = true
  }
}

function cancelEditing() {
  isEditing.value = false
  if (note.value) {
    editForm.title = note.value.title
    editForm.content = note.value.content || ''
  }
}

async function handleUpdate() {
  if (!note.value) return

  updateLoading.value = true
  try {
    await updateNote(note.value.id, {
      title: editForm.title.trim(),
      content: editForm.content.trim() || undefined,
    })
    isEditing.value = false
  } catch (err) {
    // Error handled by store
  } finally {
    updateLoading.value = false
  }
}

async function deleteNote() {
  if (!note.value || !confirm(`Are you sure you want to delete "${note.value.title}"?`)) {
    return
  }

  try {
    await deleteNoteAction(note.value.id)
    router.push('/notes')
  } catch (err) {
    // Error handled by store
  }
}

onMounted(() => {
  if (route.query.edit === 'true') {
    isEditing.value = true
  }
  fetchNote(noteId.value)
})
</script>

<style scoped>
.prose {
  max-width: none;
}

.prose p {
  margin-bottom: 1em;
}

.prose p:last-child {
  margin-bottom: 0;
}
</style>
