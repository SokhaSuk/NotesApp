<template>
  <div class="min-h-screen flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8">
    <div class="form-container p-8 space-y-8 w-full">
      <div class="text-center">
        <div class="flex justify-center">
          <img src="@/assets/images/Logo.jpg" alt="Logo" class="w-16 h-16 rounded-full">
        </div>
        <h2 class="text-3xl font-bold text-gradient mb-2">
          Welcome to TechBodia!
        </h2>
        <p class="text-gray-600 mb-8">
          Create your account to start taking notes
        </p>
        <p class="text-sm text-gray-500">
          Already have an account?
          <router-link
            to="/login"
            class="font-semibold text-gradient hover:underline ml-1 transition-all duration-300"
          >
            Sign in here
          </router-link>
        </p>
      </div>

      <form @submit.prevent="handleRegister" class="space-y-6">
        <div class="space-y-4">
          <div>
            <label for="username" class="block text-sm font-semibold text-gray-700 mb-2">
              Username
            </label>
            <input
              id="username"
              name="username"
              type="text"
              required
              v-model="form.username"
              class="input-field"
              placeholder="Choose a unique username"
              :disabled="loading"
            />
          </div>
          <div>
            <label for="email" class="block text-sm font-semibold text-gray-700 mb-2">
              Email Address
            </label>
            <input
              id="email"
              name="email"
              type="email"
              required
              v-model="form.email"
              class="input-field"
              placeholder="Enter your email address"
              :disabled="loading"
            />
          </div>
          <div>
            <label for="password" class="block text-sm font-semibold text-gray-700 mb-2">
              Password
            </label>
            <input
              id="password"
              name="password"
              type="password"
              required
              v-model="form.password"
              class="input-field"
              placeholder="Create a strong password"
              :disabled="loading"
            />
          </div>
        </div>

        <div v-if="error" class="bg-red-50 border border-red-200 rounded-xl p-4">
          <div class="flex items-center">
            <span class="text-red-700 text-sm font-medium">{{ error }}</span>
          </div>
        </div>

        <div>
          <button
            type="submit"
            class="btn-secondary w-full text-lg"
            :disabled="loading"
          >
            <LoadingSpinner v-if="loading" class="mx-auto" />
            <span v-else class="flex items-center justify-center">
              Create Account
            </span>
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import LoadingSpinner from '@/components/LoadingSpinner.vue'

const router = useRouter()
const authStore = useAuthStore()

const form = reactive({
  username: '',
  email: '',
  password: '',
})

const loading = ref(false)
const error = ref<string | null>(null)

async function handleRegister() {
  loading.value = true
  error.value = null

  try {
    await authStore.register(form)
    router.push('/notes')
  } catch (err: any) {
    error.value = err.message
  } finally {
    loading.value = false
  }
}
</script>
