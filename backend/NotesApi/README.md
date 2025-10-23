# Notes API Backend

A RESTful API built with ASP.NET Core for managing personal notes, featuring JWT authentication, user authorization, and SQL Server integration with Dapper ORM.

## Features

- 🔐 JWT Authentication & Authorization
- 📝 Full CRUD operations for notes
- 👤 User-specific note management
- 🔍 Search and filtering capabilities
- 📊 Pagination support
- 🏗️ Clean Architecture with Repository pattern
- 🗄️ SQL Server with Dapper ORM

## Tech Stack

- **Backend**: ASP.NET Core Web API
- **Database**: SQL Server
- **ORM**: Dapper (micro ORM)
- **Authentication**: JWT Bearer tokens
- **Architecture**: Clean Architecture with Repository pattern

## Project Structure

```
NotesApi/
├── Controllers/           # API controllers
│   ├── AuthController.cs  # Authentication endpoints
│   └── NotesController.cs # Notes CRUD endpoints
├── Models/               # Data models and DTOs
│   ├── Auth.cs           # Authentication models
│   ├── Note.cs           # Note models
│   └── User.cs           # User models
├── Services/             # Business logic services
│   ├── AuthService.cs    # JWT token management
│   ├── NotesService.cs   # Notes business logic
│   └── UserContext.cs    # Current user context
├── Repositories/         # Data access layer
│   ├── INotesRepository.cs
│   ├── NotesRepository.cs
│   ├── IUsersRepository.cs
│   └── UsersRepository.cs
├── Data/                 # Database configuration
│   ├── DatabaseInitializer.cs
│   └── SqlConnectionFactory.cs
├── Extensions/           # Service registration
│   └── ServiceExtensions.cs
├── Middleware/           # Custom middleware
│   ├── ExceptionHandlingMiddleware.cs
│   └── MiddlewareExtensions.cs
├── Properties/
│   └── launchSettings.json
├── appsettings.json      # Configuration
└── Program.cs           # Application startup
```

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- SQL Server (LocalDB, Express, or full edition)

### Database Setup

1. **Create the database** by running the SQL script:
   ```bash
   # In SQL Server Management Studio or Azure Data Studio
   # Run the contents of database-setup.sql
   ```

2. **Or create database manually:**
   ```sql
   CREATE DATABASE NotesDb;
   ```

3. **Update connection string** in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=NotesDb;Trusted_Connection=True;TrustServerCertificate=True;"
     }
   }
   ```

### Installation & Running

1. **Navigate to the API project:**
   ```bash
   cd backend/NotesApi
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

3. **Run database migrations:**
   The database will be automatically initialized when the application starts.

4. **Run the API:**
   ```bash
   dotnet run
   ```

5. **API will be available at:**
   - HTTP: http://localhost:5000
   - HTTPS: https://localhost:5001
   - Swagger UI: http://localhost:5000/swagger

## API Endpoints

### Authentication

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/auth/login` | User login | ❌ |
| POST | `/api/auth/register` | User registration | ❌ |
| GET | `/api/auth/me` | Get current user | ✅ |

#### Login Request
```json
{
  "username": "johndoe",
  "password": "password123"
}
```

#### Register Request
```json
{
  "username": "johndoe",
  "email": "john@example.com",
  "password": "password123"
}
```

### Notes

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/notes` | Get all user's notes | ✅ |
| GET | `/api/notes/{id}` | Get specific note | ✅ |
| POST | `/api/notes` | Create new note | ✅ |
| PUT | `/api/notes/{id}` | Update note | ✅ |
| DELETE | `/api/notes/{id}` | Delete note | ✅ |

#### Query Parameters (GET /api/notes)
- `search` - Search in title and content
- `sortBy` - Sort field: `title`, `createdAt`, `updatedAt`
- `sortOrder` - Sort order: `asc`, `desc`
- `page` - Page number (1-based)
- `pageSize` - Number of items per page

#### Create Note Request
```json
{
  "title": "My Note Title",
  "content": "Note content here..."
}
```

#### Update Note Request
```json
{
  "title": "Updated Title",
  "content": "Updated content..."
}
```

## Authentication

The API uses JWT (JSON Web Tokens) for authentication:

1. **Register** or **Login** to get a JWT token
2. **Include the token** in the Authorization header:
   ```
   Authorization: Bearer <your-jwt-token>
   ```

## Response Format

All API responses follow a consistent format:

### Success Response
```json
{
  "data": { ... },
  "message": "Success message",
  "success": true
}
```

### Error Response
```json
{
  "message": "Error message",
  "errors": {
    "field": ["Error details"]
  },
  "success": false
}
```

## Development

### Adding New Endpoints

1. **Add models** in `Models/` directory
2. **Create repository interface** in `Repositories/I*Repository.cs`
3. **Implement repository** in `Repositories/*Repository.cs`
4. **Create service** in `Services/` directory
5. **Add controller** in `Controllers/` directory
6. **Register services** in `ServiceExtensions.cs`

### Database Migrations

The application uses Dapper, so database schema changes should be managed manually:

1. **Update SQL scripts** in `database-setup.sql`
2. **Apply changes** to your database
3. **Update models** to match the new schema

## Configuration

### JWT Configuration (appsettings.json)
```json
{
  "Jwt": {
    "Key": "YourSuperSecretKeyThatIsAtLeast32CharactersLong123456789",
    "Issuer": "NotesApi",
    "Audience": "NotesApp",
    "ExpireMinutes": 60
  }
}
```

### CORS Configuration

The API is configured to allow requests from `http://localhost:3000` (frontend). Update in `Program.cs` if needed:

```csharp
services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Update as needed
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});
```

## Error Handling

The API includes comprehensive error handling:

- **400 Bad Request** - Invalid request data
- **401 Unauthorized** - Authentication required or invalid token
- **404 Not Found** - Resource not found
- **500 Internal Server Error** - Server-side errors

All errors are logged and returned in a consistent format for easy debugging.

## Security Features

- **JWT Authentication** with secure token validation
- **Password Hashing** using BCrypt
- **User Authorization** - users can only access their own notes
- **Input Validation** with DataAnnotations
- **SQL Injection Protection** via parameterized queries
- **CORS Configuration** for cross-origin requests

## Testing

### Manual Testing with Swagger

1. Start the API: `dotnet run`
2. Open http://localhost:5000/swagger
3. Test endpoints interactively

### Integration Testing

The frontend application serves as integration tests, ensuring the API works correctly with a real client application.

## Deployment

### Prerequisites for Production
- Update JWT secret key in `appsettings.json`
- Configure production database connection string
- Update CORS origins for production frontend URL
- Set appropriate logging level

### IIS Deployment
1. Publish the application: `dotnet publish -c Release`
2. Copy files to IIS directory
3. Configure application pool for .NET 8.0
4. Set up database connection

## Contributing

1. Follow the established project structure
2. Add appropriate error handling and logging
3. Update documentation for new endpoints
4. Test thoroughly before committing

## License

This project is licensed under the MIT License.
