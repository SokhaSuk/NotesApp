<template>
  <div class="max-w-5xl mx-auto">
    <!-- Loading State -->
    <div v-if="loading" class="flex justify-center py-12">
      <LoadingSpinner />
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="card max-w-2xl mx-auto">
      <div class="text-center">
        <h3 class="text-xl font-semibold text-red-600 mb-4">Oops! Something went wrong</h3>
        <p class="text-gray-600 mb-6">{{ error }}</p>
        <!-- <button @click="fetchNote" class="btn-primary">
          Try Again
        </button> -->
      </div>
    </div>

    <!-- Note Content -->
    <div v-else-if="note" class="space-y-8">
      <!-- Header -->
      <div class="text-center space-y-4">
        <div class="inline-flex items-center justify-center w-16 h-16 bg-gradient-to-r from-blue-600 to-blue-800 rounded-2xl mb-4 shadow-lg">
          <span class="text-2xl"></span>
        </div>
        <div v-if="!isEditing">
          <h1 class="text-4xl font-bold text-gradient mb-2">{{ note.title }}</h1>
          <div class="flex items-center justify-center gap-6 text-sm text-gray-500">
            <span class="flex items-center gap-2">
              Created {{ formatDate(note.createdAt) }}
            </span>
            <span v-if="note.updatedAt !== note.createdAt" class="flex items-center gap-2">
              Updated {{ formatDate(note.updatedAt) }}
            </span>
          </div>
        </div>

        <div class="flex gap-3 justify-center">
          <button
            v-if="!isEditing"
            @click="startEditing"
            class="btn-primary"
          >
            Edit Note
          </button>

          <button
            v-if="isEditing"
            @click="cancelEditing"
            class="btn-ghost"
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
      <form v-if="isEditing" @submit.prevent="handleUpdate" class="card space-y-8">
        <div>
          <label for="edit-title" class="block text-lg font-semibold text-gray-700 mb-3">
            Note Title *
          </label>
          <input
            id="edit-title"
            v-model="editForm.title"
            type="text"
            required
            class="input-field text-lg"
            placeholder="Update your note title..."
          />
        </div>

        <div>
          <label for="edit-content" class="block text-lg font-semibold text-gray-700 mb-3">
            Note Content
          </label>
          <textarea
            id="edit-content"
            v-model="editForm.content"
            rows="16"
            class="input-field resize-vertical text-lg leading-relaxed"
            placeholder="Update your note content..."
          ></textarea>
        </div>

        <div class="flex flex-col sm:flex-row gap-4 pt-6 border-t border-gray-200">
          <button
            type="submit"
            class="btn-primary flex-1 order-2 sm:order-1"
            :disabled="updateLoading"
          >
            <LoadingSpinner v-if="updateLoading" class="mx-auto" />
            <span v-else class="flex items-center justify-center">
              Save Changes
            </span>
          </button>

          <button
            type="button"
            @click="cancelEditing"
            class="btn-ghost flex-1 order-1 sm:order-2"
          >
            Cancel
          </button>
        </div>
      </form>

      <!-- View Mode -->
      <div v-else class="card">
        <div class="prose prose-lg max-w-none">
          <div v-if="note.content" class="text-gray-700 leading-relaxed whitespace-pre-wrap">
            {{ note.content }}
          </div>
          <div v-else class="text-gray-400 italic text-center py-8 text-lg">
            No content to display
          </div>
        </div>
      </div>

      <!-- Navigation -->
      <div class="text-center">
        <router-link
          to="/notes"
          class="btn-ghost inline-flex items-center"
        >
          Back to All Notes
        </router-link>
      </div>
    </div>

    <!-- Not Found -->
    <div v-else class="card max-w-2xl mx-auto">
      <div class="text-center py-16">
        <h3 class="text-2xl font-bold text-gray-900 mb-4">Note not found!</h3>
        <p class="text-gray-600 mb-8 text-lg leading-relaxed max-w-md mx-auto">
          The note you're looking for seems to have vanished into the digital ether. It might have been deleted or you might have the wrong link.
        </p>
        <router-link to="/notes" class="btn-primary text-lg inline-flex items-center">
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
  if (note) {
    editForm.title = note.title
    editForm.content = note.content || ''
    isEditing.value = true
  }
}

function cancelEditing() {
  isEditing.value = false
  if (note) {
    editForm.title = note.title
    editForm.content = note.content || ''
  }
}

async function handleUpdate() {
  if (!note) return

  updateLoading.value = true
  try {
    await updateNote(note.id, {
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
  if (!note || !confirm(`Are you sure you want to delete "${note.title}"?`)) {
    return
  }

  try {
    await deleteNoteAction(note.id)
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
