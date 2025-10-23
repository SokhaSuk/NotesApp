import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { NotesApi } from '@/api/notes'
import type { Note, CreateNoteRequest, UpdateNoteRequest, NotesResponse, NoteFilters } from '@/types/note'

export const useNotesStore = defineStore('notes', () => {
  const notes = ref<Note[]>([])
  const currentNote = ref<Note | null>(null)
  const loading = ref(false)
  const error = ref<string | null>(null)
  const filters = ref<NoteFilters>({
    page: 1,
    pageSize: 10,
    sortBy: 'createdAt',
    sortOrder: 'desc',
  })
  const totalCount = ref(0)

  const sortedNotes = computed(() => {
    return [...notes.value].sort((a, b) => {
      const sortBy = filters.value.sortBy || 'createdAt'
      const sortOrder = filters.value.sortOrder || 'desc'

      let aValue: any = a[sortBy]
      let bValue: any = b[sortBy]

      if (sortBy === 'createdAt' || sortBy === 'updatedAt') {
        aValue = new Date(aValue).getTime()
        bValue = new Date(bValue).getTime()
      }

      if (sortOrder === 'asc') {
        return aValue > bValue ? 1 : -1
      } else {
        return aValue < bValue ? 1 : -1
      }
    })
  })

  const filteredNotes = computed(() => {
    if (!filters.value.search) return sortedNotes.value

    const searchTerm = filters.value.search.toLowerCase()
    return sortedNotes.value.filter(note =>
      note.title.toLowerCase().includes(searchTerm) ||
      (note.content && note.content.toLowerCase().includes(searchTerm))
    )
  })

  async function fetchNotes() {
    loading.value = true
    error.value = null

    try {
      const response = await NotesApi.getNotes(filters.value)
      notes.value = response.notes
      totalCount.value = response.totalCount
    } catch (err: any) {
      error.value = err.message
    } finally {
      loading.value = false
    }
  }

  async function fetchNote(id: number) {
    loading.value = true
    error.value = null

    try {
      currentNote.value = await NotesApi.getNote(id)
    } catch (err: any) {
      error.value = err.message
    } finally {
      loading.value = false
    }
  }

  async function createNote(noteData: CreateNoteRequest) {
    loading.value = true
    error.value = null

    try {
      const newNote = await NotesApi.createNote(noteData)
      notes.value.unshift(newNote)
      totalCount.value++
      return newNote
    } catch (err: any) {
      error.value = err.message
      throw err
    } finally {
      loading.value = false
    }
  }

  async function updateNote(id: number, noteData: UpdateNoteRequest) {
    loading.value = true
    error.value = null

    try {
      const updatedNote = await NotesApi.updateNote(id, noteData)

      const index = notes.value.findIndex(note => note.id === id)
      if (index !== -1) {
        notes.value[index] = updatedNote
      }

      if (currentNote.value && currentNote.value.id === id) {
        currentNote.value = updatedNote
      }

      return updatedNote
    } catch (err: any) {
      error.value = err.message
      throw err
    } finally {
      loading.value = false
    }
  }

  async function deleteNote(id: number) {
    loading.value = true
    error.value = null

    try {
      await NotesApi.deleteNote(id)
      notes.value = notes.value.filter(note => note.id !== id)
      totalCount.value--

      if (currentNote.value && currentNote.value.id === id) {
        currentNote.value = null
      }
    } catch (err: any) {
      error.value = err.message
      throw err
    } finally {
      loading.value = false
    }
  }

  function setFilters(newFilters: Partial<NoteFilters>) {
    filters.value = { ...filters.value, ...newFilters }
  }

  function resetFilters() {
    filters.value = {
      page: 1,
      pageSize: 10,
      sortBy: 'createdAt',
      sortOrder: 'desc',
    }
  }

  return {
    notes,
    currentNote,
    loading,
    error,
    filters,
    totalCount,
    filteredNotes,
    fetchNotes,
    fetchNote,
    createNote,
    updateNote,
    deleteNote,
    setFilters,
    resetFilters,
  }
})
