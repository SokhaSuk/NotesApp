# Notes App Frontend

A modern Vue.js application for managing personal notes with TypeScript, Tailwind CSS, and Pinia for state management.

## Features

- 🔐 User authentication (login/register)
- 📝 Full CRUD operations for notes
- 🔍 Search and filtering functionality
- 📱 Responsive design with Tailwind CSS
- 🎯 TypeScript for type safety
- 🚀 Built with Vue 3 + Vite

## Tech Stack

- **Frontend**: Vue 3, TypeScript, Tailwind CSS
- **State Management**: Pinia
- **Routing**: Vue Router
- **HTTP Client**: Axios
- **Build Tool**: Vite

## Project Structure

```
src/
├── api/           # API service layer
│   ├── auth.ts    # Authentication API calls
│   ├── http.ts    # HTTP client configuration
│   └── notes.ts   # Notes API calls
├── components/    # Reusable Vue components
│   ├── LoadingSpinner.vue
│   └── NavBar.vue
├── stores/        # Pinia stores
│   ├── auth.ts    # Authentication state
│   └── notes.ts   # Notes state management
├── types/         # TypeScript type definitions
│   ├── api.ts     # API response types
│   ├── note.ts    # Note-related types
│   └── user.ts    # User-related types
├── views/         # Vue views/pages
│   ├── LoginView.vue
│   ├── RegisterView.vue
│   ├── NotesListView.vue
│   ├── NoteCreateView.vue
│   └── NoteDetailView.vue
├── router/        # Vue Router configuration
└── main.ts        # Application entry point
```

## Getting Started

### Prerequisites

- Node.js (v18 or higher)
- npm or yarn

### Installation

1. **Install dependencies**
   ```bash
   cd frontend
   npm install
   ```

2. **Start the development server**
   ```bash
   npm run dev
   ```

3. **Build for production**
   ```bash
   npm run build
   ```

4. **Preview production build**
   ```bash
   npm run preview
   ```

## Environment Configuration

The application is configured to proxy API calls to `http://localhost:5000`. Make sure your backend API is running on this port.

## Available Scripts

- `npm run dev` - Start development server
- `npm run build` - Build for production
- `npm run preview` - Preview production build
- `npm run type-check` - Run TypeScript type checking

## API Integration

The frontend communicates with the backend API through:

- **Authentication endpoints**: `/api/auth/login`, `/api/auth/register`, `/api/auth/me`
- **Notes endpoints**: `/api/notes` (GET, POST, PUT, DELETE)

## Features Overview

### Authentication
- User registration and login
- JWT token management
- Protected routes

### Notes Management
- Create, read, update, delete notes
- Search functionality
- Sorting by date or title
- Pagination support
- Responsive design

### State Management
- Pinia stores for auth and notes
- Centralized API error handling
- Loading states

## Development

The project uses Vue 3 with Composition API, TypeScript for type safety, and Tailwind CSS for styling. The state is managed using Pinia, and routing is handled by Vue Router.

Make sure to update the backend API endpoints in the configuration if your backend runs on a different port or path.
