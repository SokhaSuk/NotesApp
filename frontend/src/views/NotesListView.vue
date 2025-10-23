<template>
  <div class="space-y-6">
    <!-- Header -->
    <div class="flex justify-between items-center">
      <h1 class="text-3xl font-bold text-gray-900">My Notes</h1>
      <router-link to="/notes/create" class="btn-primary">
        Create Note
      </router-link>
    </div>

    <!-- Search and Filters -->
    <div class="card">
      <div class="flex flex-col sm:flex-row gap-4">
        <!-- Search -->
        <div class="flex-1">
          <input
            v-model="searchQuery"
            type="text"
            placeholder="Search notes..."
            class="input-field"
            @input="handleSearch"
          />
        </div>

        <!-- Sort Options -->
        <div class="flex gap-2">
          <select
            v-model="sortBy"
            @change="handleSort"
            class="input-field"
          >
            <option value="createdAt">Date Created</option>
            <option value="updatedAt">Date Updated</option>
            <option value="title">Title</option>
          </select>

          <select
            v-model="sortOrder"
            @change="handleSort"
            class="input-field"
          >
            <option value="desc">Descending</option>
            <option value="asc">Ascending</option>
          </select>
        </div>
      </div>
    </div>

    <!-- Loading State -->
    <div v-if="loading" class="flex justify-center py-8">
      <LoadingSpinner />
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="card">
      <div class="text-red-600 text-center">
        <p>{{ error }}</p>
        <button @click="fetchNotes" class="btn-secondary mt-2">
          Try Again
        </button>
      </div>
    </div>

    <!-- Notes List -->
    <div v-else-if="notes.length > 0" class="space-y-4">
      <div
        v-for="note in notes"
        :key="note.id"
        class="card hover:shadow-lg transition-shadow cursor-pointer"
        @click="viewNote(note.id)"
      >
        <div class="flex justify-between items-start">
          <div class="flex-1">
            <h3 class="text-xl font-semibold text-gray-900 mb-2">
              {{ note.title }}
            </h3>
            <p v-if="note.content" class="text-gray-600 line-clamp-2 mb-3">
              {{ note.content }}
            </p>
            <div class="text-sm text-gray-500">
              <span>Created: {{ formatDate(note.createdAt) }}</span>
              <span v-if="note.updatedAt !== note.createdAt" class="ml-4">
                Updated: {{ formatDate(note.updatedAt) }}
              </span>
            </div>
          </div>
          <div class="flex gap-2 ml-4">
            <button
              @click.stop="editNote(note.id)"
              class="btn-secondary text-sm"
            >
              Edit
            </button>
            <button
              @click.stop="deleteNote(note)"
              class="btn-danger text-sm"
            >
              Delete
            </button>
          </div>
        </div>
      </div>

      <!-- Pagination -->
      <div class="flex justify-center mt-6" v-if="totalPages > 1">
        <div class="flex gap-2">
          <button
            @click="changePage(currentPage - 1)"
            :disabled="currentPage === 1"
            class="btn-secondary"
          >
            Previous
          </button>

          <span class="flex items-center px-4 text-gray-600">
            Page {{ currentPage }} of {{ totalPages }}
          </span>

          <button
            @click="changePage(currentPage + 1)"
            :disabled="currentPage === totalPages"
            class="btn-secondary"
          >
            Next
          </button>
        </div>
      </div>
    </div>

    <!-- Empty State -->
    <div v-else class="card">
      <div class="text-center py-12">
        <div class="text-gray-400 mb-4">
          <svg class="mx-auto h-12 w-12" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
          </svg>
        </div>
        <h3 class="text-lg font-medium text-gray-900 mb-2">No notes yet</h3>
        <p class="text-gray-500 mb-4">Get started by creating your first note.</p>
        <router-link to="/notes/create" class="btn-primary">
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
const currentPage = computed(() => filters.page || 1)
const totalPages = computed(() => Math.ceil(totalCount.value / (filters.pageSize || 10)))

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

<style scoped>
.line-clamp-2 {
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}
</style>
