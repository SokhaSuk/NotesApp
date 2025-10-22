<script setup lang="ts">
import { onMounted, onUnmounted, ref } from 'vue';
import { subscribe, removeToast, type Toast } from '../lib/toast';

const toasts = ref<Toast[]>([]);

onMounted(() => {
  const unsubscribe = subscribe((newToasts) => {
    toasts.value = newToasts;
  });

  onUnmounted(() => {
    unsubscribe();
  });
});

function getToastIcon(type: Toast['type']) {
  switch (type) {
    case 'success':
      return 'M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z';
    case 'error':
      return 'M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z';
    case 'warning':
      return 'M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-2.5L13.732 4c-.77-.833-1.964-.833-2.732 0L3.732 16.5c-.77.833.192 2.5 1.732 2.5z';
    case 'info':
    default:
      return 'M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z';
  }
}

function getToastStyles(type: Toast['type']) {
  switch (type) {
    case 'success':
      return 'border-green-200 bg-green-50 dark:bg-green-900/20 dark:border-green-800';
    case 'error':
      return 'border-red-200 bg-red-50 dark:bg-red-900/20 dark:border-red-800';
    case 'warning':
      return 'border-yellow-200 bg-yellow-50 dark:bg-yellow-900/20 dark:border-yellow-800';
    case 'info':
    default:
      return 'border-blue-200 bg-blue-50 dark:bg-blue-900/20 dark:border-blue-800';
  }
}

function getToastIconColor(type: Toast['type']) {
  switch (type) {
    case 'success':
      return 'text-green-600';
    case 'error':
      return 'text-red-600';
    case 'warning':
      return 'text-yellow-600';
    case 'info':
    default:
      return 'text-blue-600';
  }
}
</script>

<template>
  <div class="toast">
    <transition-group name="toast" tag="div" class="space-y-2">
      <div
        v-for="toast in toasts"
        :key="toast.id"
        :class="[
          'toast-item border-l-4 flex items-start gap-3',
          getToastStyles(toast.type)
        ]"
      >
        <div class="flex-shrink-0 mt-0.5">
          <svg class="w-5 h-5" :class="getToastIconColor(toast.type)" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" :d="getToastIcon(toast.type)" />
          </svg>
        </div>
        <div class="flex-1 min-w-0">
          <p class="text-sm font-medium text-gray-900 dark:text-gray-100">
            {{ toast.message }}
          </p>
        </div>
        <button
          @click="removeToast(toast.id)"
          class="flex-shrink-0 text-gray-400 hover:text-gray-600 dark:hover:text-gray-300 transition-colors"
        >
          <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
          </svg>
        </button>
      </div>
    </transition-group>
  </div>
</template>

<style scoped>
.toast-enter-active,
.toast-leave-active {
  transition: all 0.3s ease;
}

.toast-enter-from {
  opacity: 0;
  transform: translateX(100%);
}

.toast-leave-to {
  opacity: 0;
  transform: translateX(100%);
}

.toast-move {
  transition: transform 0.3s ease;
}
</style>
