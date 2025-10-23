<template>
  <nav class="nav-glass sticky top-0 z-50">
    <div class="container mx-auto px-6">
      <div class="flex justify-between items-center h-20">
        <div class="flex items-center">
          <router-link
            to="/notes"
            class="text-2xl font-bold text-gradient hover:scale-105 transition-transform duration-300"
          >
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

          <div class="flex items-center space-x-3">
            <div class="flex items-center space-x-2 bg-white/20 rounded-full px-4 py-2 backdrop-blur-sm">
              <div class="w-8 h-8 rounded-full bg-gradient-to-r from-blue-600 to-blue-800 flex items-center justify-center text-white font-semibold text-sm">
                {{ authStore.user?.username?.charAt(0).toUpperCase() }}
              </div>
              <span class="text-white font-medium">{{ authStore.user?.username }}</span>
            </div>
            <button
              @click="logout"
              class="btn-ghost text-sm"
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
