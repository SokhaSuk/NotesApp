# Notes App Frontend

A modern Vue.js application for managing personal notes with TypeScript, Tailwind CSS, and Pinia for state management.

## Features

- 🔐 User authentication (login/register)
- 📝 Full CRUD operations for notes
- 🔍 Search and filtering functionality
- 📱 Responsive design with Tailwind CSS
- 🎨 **Modern Glass Morphism UI** with gradients and animations
- 🌈 **Contemporary Color Palette** with purple/pink/blue gradients
- ✨ **Interactive Elements** with hover effects and smooth transitions
- 🎭 **Emoji Integration** for better user experience
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

## ✨ What You'll See

When you run the application, you'll experience:

- **🚀 Professional Login/Register Forms**: Glass morphism cards with animated backgrounds and professional gradient icons
- **🎨 Modern Navigation**: Glass-style navbar with user avatars and professional branding
- **💎 Beautiful Note Cards**: Hover effects, professional gradient indicators, and smooth animations
- **🔍 Interactive Search**: Glass-style search bar with emoji icons and real-time filtering
- **🎭 Engaging Empty States**: Friendly messages with bouncing animations and call-to-action buttons
- **🌈 Professional Color Scheme**: Navy-to-dark-red gradients throughout with professional accent colors

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
   Open http://localhost:3000 to see the modern UI in action! 🎉

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

## 🎨 Modern UI Features

### Visual Design
- **Glass Morphism Effects**: Translucent cards with backdrop blur
- **Gradient Backgrounds**: Beautiful purple to blue gradients
- **Modern Typography**: Clean, readable fonts with gradient text effects
- **Smooth Animations**: Hover effects, scale transforms, and transitions
- **Emoji Integration**: Contextual icons for better user experience

### Interactive Elements
- **Dynamic Buttons**: Gradient backgrounds with glow effects on hover
- **Card Animations**: Lift effects and enhanced shadows on interaction
- **Loading States**: Custom animated spinners with modern styling
- **Form Enhancements**: Glass-style inputs with focus animations

### Color Palette
- **Primary**: Navy to Dark Blue gradients (#003049 → #1a3d5c)
- **Secondary**: Dark Red to Maroon gradients (#780000 → #8b0000)
- **Accent**: Blue gradients (#003049 → #00223a)
- **Neutrals**: Semi-transparent whites with backdrop blur

## Features Overview

### Authentication
- User registration and login with glass morphism forms
- JWT token management with secure storage
- Protected routes with modern loading states
- User avatars with gradient backgrounds

### Notes Management
- Create, read, update, delete notes with modern card layouts
- Real-time search with glass-style search bar
- Sorting by date or title with emoji indicators
- Pagination with modern glass navigation
- Responsive grid layout for all screen sizes

### State Management
- Pinia stores for auth and notes with modern error handling
- Centralized API integration with loading indicators
- Optimistic updates with smooth transitions

## Development

The project uses Vue 3 with Composition API, TypeScript for type safety, and Tailwind CSS for styling. The state is managed using Pinia, and routing is handled by Vue Router.

Make sure to update the backend API endpoints in the configuration if your backend runs on a different port or path.
