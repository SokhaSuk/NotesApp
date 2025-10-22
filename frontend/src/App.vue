<script setup lang="ts">
import ToastContainer from '@/components/ToastContainer.vue';
import NavBar from '@/components/NavBar.vue';
</script>

<template>
  <div class="min-h-screen gradient-bg relative overflow-hidden">
    <!-- Background decorative elements -->
    <div class="absolute inset-0 overflow-hidden pointer-events-none">
      <div class="absolute -top-40 -right-40 w-80 h-80 bg-gradient-to-br from-blue-400/20 to-indigo-500/20 rounded-full blur-3xl animate-float"></div>
      <div class="absolute -bottom-40 -left-40 w-80 h-80 bg-gradient-to-br from-purple-400/20 to-pink-500/20 rounded-full blur-3xl animate-float" style="animation-delay: -1.5s; animation-duration: 3s;"></div>
      <div class="absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 w-96 h-96 bg-gradient-to-br from-cyan-400/10 to-blue-500/10 rounded-full blur-3xl animate-pulse-glow"></div>
    </div>

    <NavBar />
    <main class="relative z-10">
      <div class="container mx-auto max-w-5xl px-4 py-8">
        <router-view />
      </div>
    </main>
    <ToastContainer />
  </div>
</template>
