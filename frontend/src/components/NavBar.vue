<script setup lang="ts">
import { onMounted, ref } from 'vue';

const dark = ref<boolean>(false);

onMounted(() => {
  const saved = localStorage.getItem('theme');
  dark.value = saved ? saved === 'dark' : window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
  document.documentElement.classList.toggle('dark', dark.value);
});

function toggleDark() {
  dark.value = !dark.value;
  document.documentElement.classList.toggle('dark', dark.value);
  localStorage.setItem('theme', dark.value ? 'dark' : 'light');
}
</script>

<template>
  <header class="border-b border-gray-200/50 dark:border-gray-700/50 bg-white/80 dark:bg-gray-900/80 backdrop-blur-md shadow-sm">
    <div class="container mx-auto max-w-4xl px-4 h-16 flex items-center justify-between">
      <div class="flex items-center gap-3">
        <div class="flex items-center gap-2">
          <img src="@/assets/Logo.jpg" alt="Company Logo" class="w-8 h-8 rounded-lg shadow-sm" />
          <div class="flex flex-col">
            <span class="font-bold text-lg text-gray-900 dark:text-white">NotesApp</span>
            <span class="text-xs text-gray-500 dark:text-gray-400 -mt-1">Smart Notes</span>
          </div>
        </div>
      </div>

      <div class="flex items-center gap-3">
        <div class="hidden sm:flex items-center gap-2 text-sm text-gray-600 dark:text-gray-400">
          <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 7h.01M7 3h5c.512 0 1.024.195 1.414.586l7 7a2 2 0 010 2.828l-7 7a2 2 0 01-2.828 0l-7-7A1.994 1.994 0 013 12V7a4 4 0 014-4z" />
          </svg>
          <span>Organize your thoughts</span>
        </div>

        <button
          class="btn btn-secondary h-9 px-4 rounded-full border-0 bg-gray-100 dark:bg-gray-800 hover:bg-gray-200 dark:hover:bg-gray-700 transition-all duration-200"
          @click="toggleDark"
        >
          <svg v-if="!dark" class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20.354 15.354A9 9 0 018.646 3.646 9.003 9.003 0 0012 21a9.003 9.003 0 008.354-5.646z" />
          </svg>
          <svg v-else class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 3v1m0 16v1m9-9h-1M4 12H3m15.364 6.364l-.707-.707M6.343 6.343l-.707-.707m12.728 0l-.707.707M6.343 17.657l-.707.707M16 12a4 4 0 11-8 0 4 4 0 018 0z" />
          </svg>
        </button>
      </div>
    </div>
  </header>
</template>


