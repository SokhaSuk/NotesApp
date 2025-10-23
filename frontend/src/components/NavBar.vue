<template>
  <nav class="bg-white shadow-lg">
    <div class="container mx-auto px-4">
      <div class="flex justify-between items-center h-16">
        <div class="flex items-center">
          <router-link to="/notes" class="text-xl font-bold text-gray-800">
            Notes App
          </router-link>
        </div>

        <div class="flex items-center space-x-4">
          <router-link
            to="/notes/create"
            class="btn-primary text-sm"
          >
            New Note
          </router-link>

          <div class="flex items-center space-x-2">
            <span class="text-gray-600">{{ authStore.user?.username }}</span>
            <button
              @click="logout"
              class="btn-secondary text-sm"
              :disabled="authStore.loading"
            >
              Logout
            </button>
          </div>
        </div>
      </div>
    </div>
  </nav>
</template>

<script setup lang="ts">
import { useAuthStore } from '@/stores/auth'

const authStore = useAuthStore()

function logout() {
  authStore.logout()
}
</script>
