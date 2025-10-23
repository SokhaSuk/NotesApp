<template>
  <div class="space-y-8">
    <!-- Header -->
    <div class="text-center space-y-4">
      <h1 class="text-5xl font-bold text-gradient mb-4">
        My Notes
      </h1>
      <p class="text-gray-600 text-lg max-w-2xl mx-auto">
        Capture your thoughts, ideas, and memories in one beautiful place
      </p>
      <router-link to="/notes/create" class="btn-primary inline-flex items-center">
        Create New Note
      </router-link>
    </div>

    <!-- Search and Filters -->
    <div class="card max-w-4xl mx-auto">
      <div class="flex flex-col sm:flex-row gap-4">
        <!-- Search -->
        <div class="flex-1">
          <div class="relative">
            <input
              v-model="searchQuery"
              type="text"
              placeholder="Search your notes..."
              class="input-field pl-4"
              @input="handleSearch"
            />
          </div>
        </div>

        <!-- Sort Options -->
        <div class="flex gap-3">
          <div class="relative">
            <select
              v-model="sortBy"
              @change="handleSort"
              class="input-field bg-white/50 backdrop-blur-sm"
            >
              <option value="createdAt">Date Created</option>
              <option value="updatedAt">Date Updated</option>
              <option value="title">Title</option>
            </select>
          </div>

          <div class="relative">
            <select
              v-model="sortOrder"
              @change="handleSort"
              class="input-field bg-white/50 backdrop-blur-sm"
            >
              <option value="desc">Descending</option>
              <option value="asc">Ascending</option>
            </select>
          </div>
        </div>
      </div>
    </div>

    <!-- Loading State -->
    <div v-if="loading" class="flex justify-center py-12">
      <LoadingSpinner />
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="card max-w-2xl mx-auto">
      <div class="text-center">
        <h3 class="text-xl font-semibold text-red-600 mb-4">Oops! Something went wrong</h3>
        <p class="text-gray-600 mb-6">{{ error }}</p>
        <button @click="fetchNotes" class="btn-primary">
          Try Again
        </button>
      </div>
    </div>

    <!-- Notes List -->
    <div v-else-if="notes.length > 0" class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      <div
        v-for="note in notes"
        :key="note.id"
        class="note-card group"
        @click="viewNote(note.id)"
      >
        <div class="flex justify-between items-start mb-4">
          <div class="flex-1">
            <div class="flex items-center gap-2 mb-3">
              <div class="w-3 h-3 rounded-full bg-gradient-to-r from-blue-600 to-blue-800"></div>
              <h3 class="text-xl font-bold text-gray-900 group-hover:text-gradient transition-all duration-300">
                {{ note.title }}
              </h3>
            </div>
            <p v-if="note.content" class="text-gray-600 line-clamp-2 mb-4 leading-relaxed">
              {{ note.content }}
            </p>
            <div class="flex items-center gap-4 text-sm text-gray-500">
              <span>
                Created {{ formatDate(note.createdAt) }}
              </span>
              <span v-if="note.updatedAt !== note.createdAt">
                Updated {{ formatDate(note.updatedAt) }}
              </span>
            </div>
          </div>
        </div>

        <div class="flex gap-2 mt-4 opacity-0 group-hover:opacity-100 transition-opacity duration-300">
          <button
            @click.stop="editNote(note.id)"
            class="btn-secondary text-sm flex-1"
          >
            Edit
          </button>
          <button
            @click.stop="deleteNote(note)"
            class="btn-danger text-sm flex-1"
          >
            Delete
          </button>
        </div>
      </div>
    </div>

    <!-- Pagination -->
    <div class="flex justify-center mt-12" v-if="totalPages > 1">
      <div class="flex items-center gap-4 bg-white/50 backdrop-blur-sm rounded-2xl p-4 shadow-lg">
        <button
          @click="changePage(currentPage - 1)"
          :disabled="currentPage === 1"
          class="btn-ghost disabled:opacity-50 disabled:cursor-not-allowed"
        >
          Previous
        </button>

        <div class="flex items-center gap-2">
          <span class="px-4 py-2 bg-white/80 rounded-xl font-semibold text-gray-700">
            {{ currentPage }} of {{ totalPages }}
          </span>
        </div>

        <button
          @click="changePage(currentPage + 1)"
          :disabled="currentPage === totalPages"
          class="btn-ghost disabled:opacity-50 disabled:cursor-not-allowed"
        >
          Next
        </button>
      </div>
    </div>

    <!-- Empty State -->
    <div v-else class="card max-w-2xl mx-auto">
      <div class="text-center py-16">
        <h3 class="text-2xl font-bold text-gray-900 mb-4">No notes yet!</h3>
        <p class="text-gray-600 mb-8 text-lg leading-relaxed max-w-md mx-auto">
          Your creative space is waiting. Start capturing your thoughts, ideas, and inspirations with your first note.
        </p>
        <router-link to="/notes/create" class="btn-primary text-lg inline-flex items-center">
          Create Your First Note
        </router-link>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useNotesStore } from '@/stores/notes'
import LoadingSpinner from '@/components/LoadingSpinner.vue'

const router = useRouter()
const notesStore = useNotesStore()

const searchQuery = ref('')
const sortBy = ref<'title' | 'createdAt' | 'updatedAt'>('createdAt')
const sortOrder = ref<'asc' | 'desc'>('desc')

const { notes, loading, error, filters, totalCount, fetchNotes } = notesStore
const currentPage = computed(() => filters?.page || 1)
const totalPages = computed(() => Math.ceil(totalCount / (filters?.pageSize || 10)))
function formatDate(dateString: string): string {
  return new Date(dateString).toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  })
}

function handleSearch() {
  notesStore.setFilters({ search: searchQuery.value, page: 1 })
  fetchNotes()
}

function handleSort() {
  notesStore.setFilters({ sortBy: sortBy.value, sortOrder: sortOrder.value, page: 1 })
  fetchNotes()
}

function changePage(page: number) {
  notesStore.setFilters({ page })
  fetchNotes()
}

function viewNote(id: number) {
  router.push(`/notes/${id}`)
}

function editNote(id: number) {
  router.push(`/notes/${id}?edit=true`)
}

async function deleteNote(note: any) {
  if (confirm(`Are you sure you want to delete "${note.title}"?`)) {
    try {
      await notesStore.deleteNote(note.id)
    } catch (err) {
      // Error is handled by the store
    }
  }
}

onMounted(() => {
  fetchNotes()
})
</script>

