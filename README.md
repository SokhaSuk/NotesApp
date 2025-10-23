# Notes App - Full Stack Application

A complete full-stack notes management application built with Vue.js + TypeScript frontend and ASP.NET Core Web API backend, featuring user authentication, CRUD operations, and modern development practices.

## 🎯 Features

### Frontend (Vue.js + TypeScript)
- 🔐 User authentication (login/register)
- 📝 Full CRUD operations for notes
- 🔍 Search and filtering functionality
- 📱 Responsive design with Tailwind CSS
- 🎯 TypeScript for type safety
- 🚀 Built with Vue 3 + Vite
- 📊 State management with Pinia
- 🛣️ Vue Router for navigation

### Backend (ASP.NET Core)
- 🔐 JWT Authentication & Authorization
- 📝 Full CRUD operations for notes
- 👤 User-specific note management
- 🔍 Search and filtering capabilities
- 📊 Pagination support
- 🏗️ Clean Architecture with Repository pattern
- 🗄️ SQL Server with Dapper ORM

## 🛠️ Tech Stack

### Frontend
- **Framework**: Vue 3 (Composition API)
- **Language**: TypeScript
- **Styling**: Tailwind CSS
- **State Management**: Pinia
- **Routing**: Vue Router
- **HTTP Client**: Axios
- **Build Tool**: Vite
- **Package Manager**: npm

### Backend
- **Framework**: ASP.NET Core Web API
- **Language**: C#
- **Database**: SQL Server
- **ORM**: Dapper (micro ORM)
- **Authentication**: JWT Bearer tokens
- **Architecture**: Clean Architecture
- **Build Tool**: .NET CLI

## 📁 Project Structure

```
NotesApp/
├── frontend/              # Vue.js Frontend
│   ├── src/
│   │   ├── api/          # API service layer
│   │   ├── components/   # Reusable Vue components
│   │   ├── stores/       # Pinia stores
│   │   ├── types/        # TypeScript definitions
│   │   ├── views/        # Vue pages/views
│   │   └── router/       # Vue Router config
│   ├── public/           # Static assets
│   └── package.json      # Dependencies
├── backend/              # ASP.NET Core API
│   └── NotesApi/
│       ├── Controllers/  # API endpoints
│       ├── Models/       # Data models
│       ├── Services/     # Business logic
│       ├── Repositories/ # Data access layer
│       ├── Data/         # Database config
│       └── Properties/   # Project settings
└── database-setup.sql    # Database schema
```

## 🚀 Quick Start

### Prerequisites
- Node.js (v18 or higher)
- .NET 8.0 SDK or later
- SQL Server (LocalDB, Express, or full edition)

### 1. Database Setup

Run the database setup script:
```bash
# In SQL Server Management Studio or Azure Data Studio
# Execute the contents of backend/database-setup.sql
```

Or manually create the database:
```sql
CREATE DATABASE NotesDb;
```

### 2. Backend Setup

```bash
# Navigate to backend directory
cd backend/NotesApi

# Restore dependencies
dotnet restore

# Run the API (database will be auto-initialized)
dotnet run
```

The API will be available at:
- HTTP: http://localhost:5000
- HTTPS: https://localhost:5001
- Swagger UI: http://localhost:5000/swagger

### 3. Frontend Setup

```bash
# Navigate to frontend directory
cd frontend

# Install dependencies
npm install

# Start development server
npm run dev
```

The frontend will be available at:
- http://localhost:3000

## 🔧 Configuration

### Backend Configuration

Update `backend/NotesApi/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=NotesDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "YourSuperSecretKeyThatIsAtLeast32CharactersLong123456789",
    "Issuer": "NotesApi",
    "Audience": "NotesApp",
    "ExpireMinutes": 60
  }
}
```

### Frontend Configuration

The frontend is configured to proxy API calls to `http://localhost:5000`. Update `frontend/vite.config.ts` if needed:

```typescript
server: {
  proxy: {
    '/api': {
      target: 'http://localhost:5000', // Your backend URL
      changeOrigin: true
    }
  }
}
```

## 📚 API Documentation

### Authentication Endpoints

| Method | Endpoint | Description | Body |
|--------|----------|-------------|------|
| POST | `/api/auth/login` | User login | `{username, password}` |
| POST | `/api/auth/register` | User registration | `{username, email, password}` |
| GET | `/api/auth/me` | Get current user | - |

### Notes Endpoints

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/notes` | Get all user's notes | ✅ |
| GET | `/api/notes/{id}` | Get specific note | ✅ |
| POST | `/api/notes` | Create new note | ✅ |
| PUT | `/api/notes/{id}` | Update note | ✅ |
| DELETE | `/api/notes/{id}` | Delete note | ✅ |

### Query Parameters (GET /api/notes)
- `search` - Search in title and content
- `sortBy` - `title`, `createdAt`, `updatedAt`
- `sortOrder` - `asc`, `desc`
- `page` - Page number
- `pageSize` - Items per page

## 🔐 Authentication Flow

1. **Register** or **Login** via frontend forms
2. **Backend** validates credentials and returns JWT token
3. **Frontend** stores token in localStorage
4. **Subsequent requests** include `Authorization: Bearer <token>` header
5. **Backend** validates token and sets user context
6. **Users** can only access their own notes

## 🎨 Frontend Features

### Authentication
- User registration and login forms
- JWT token management
- Protected routes
- Automatic token refresh

### Notes Management
- Create, read, update, delete notes
- Real-time search functionality
- Sorting by date or title
- Pagination support
- Responsive design

### State Management
- Pinia stores for auth and notes
- Centralized error handling
- Loading states
- Optimistic updates

## 🏗️ Backend Features

### Security
- JWT authentication with secure validation
- Password hashing with BCrypt
- User authorization (users see only their notes)
- Input validation and sanitization
- CORS protection

### Database
- SQL Server integration
- Dapper micro-ORM for performance
- Repository pattern for data access
- Automatic database initialization
- Optimized queries with indexes

### Architecture
- Clean Architecture principles
- Dependency injection
- Middleware for error handling
- Service layer for business logic
- Repository pattern for data persistence

## 🧪 Development & Testing

### Frontend Development
```bash
cd frontend
npm run dev        # Start dev server
npm run build      # Build for production
npm run preview    # Preview production build
npm run type-check # TypeScript checking
```

### Backend Development
```bash
cd backend/NotesApi
dotnet run         # Start API server
dotnet build       # Build project
dotnet test        # Run tests (when added)
```

### Testing the Integration
1. Start both frontend and backend servers
2. Register a new user or login
3. Create, edit, and delete notes
4. Test search and filtering
5. Verify responsive design on different screen sizes

## 🚀 Deployment

### Frontend Deployment
```bash
cd frontend
npm run build
# Deploy the dist/ folder to your web server
```

### Backend Deployment
```bash
cd backend/NotesApi
dotnet publish -c Release -o publish
# Deploy the publish/ folder to your server
# Configure IIS or use as self-contained deployment
```

### Production Considerations
- Update JWT secret key
- Configure production database
- Set up proper CORS origins
- Configure logging levels
- Set up SSL certificates

## 🐛 Troubleshooting

### Common Issues

1. **Database Connection Issues**
   - Verify SQL Server is running
   - Check connection string in appsettings.json
   - Ensure database exists and user has permissions

2. **CORS Issues**
   - Verify frontend URL in backend CORS configuration
   - Check if API proxy is working in frontend

3. **Authentication Issues**
   - Check JWT configuration in appsettings.json
   - Verify token expiration settings
   - Check browser console for token errors

4. **Build Issues**
   - Clear node_modules and reinstall: `rm -rf node_modules && npm install`
   - Clear .NET build cache: `dotnet clean`

## 🤝 Contributing

1. Follow the established project structure
2. Add appropriate error handling
3. Update documentation for new features
4. Test thoroughly before committing
5. Follow TypeScript and C# best practices

## 📄 License

This project is licensed under the MIT License.

## 🆘 Support

If you encounter issues:

1. Check the troubleshooting section above
2. Review the API documentation
3. Check browser console and server logs
4. Verify all prerequisites are installed correctly

## 🔄 Next Steps

- Add unit and integration tests
- Implement email verification
- Add note categories/tags
- Implement file attachments
- Add dark mode toggle
- Add note sharing functionality
- Implement real-time updates with SignalR
- Add export functionality (PDF, JSON)

---

**Happy coding!** 🎉
