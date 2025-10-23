using Dapper;
using Microsoft.Data.Sqlite;

namespace NotesApi.Data;

public class DatabaseInitializer
{
    private readonly DatabaseConnectionFactory _connectionFactory;

    public DatabaseInitializer(DatabaseConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task InitializeAsync()
    {
        using var connection = _connectionFactory.CreateSqliteConnection();

        await connection.ExecuteAsync(@"
            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT NOT NULL UNIQUE,
                Email TEXT NOT NULL UNIQUE,
                PasswordHash TEXT NOT NULL,
                CreatedAt TEXT NOT NULL DEFAULT (datetime('now'))
            )");

        await connection.ExecuteAsync(@"
            CREATE TABLE IF NOT EXISTS Notes (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                Content TEXT,
                CreatedAt TEXT NOT NULL DEFAULT (datetime('now')),
                UpdatedAt TEXT NOT NULL DEFAULT (datetime('now')),
                UserId INTEGER NOT NULL,
                FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
            )");

        // Create indexes for better performance
        await connection.ExecuteAsync(@"
            CREATE INDEX IF NOT EXISTS IX_Notes_UserId ON Notes(UserId)");

        await connection.ExecuteAsync(@"
            CREATE INDEX IF NOT EXISTS IX_Notes_CreatedAt ON Notes(CreatedAt DESC)");

        await connection.ExecuteAsync(@"
            CREATE INDEX IF NOT EXISTS IX_Notes_UpdatedAt ON Notes(UpdatedAt DESC)");

        await connection.ExecuteAsync(@"
            CREATE INDEX IF NOT EXISTS IX_Users_Username ON Users(Username)");

        await connection.ExecuteAsync(@"
            CREATE INDEX IF NOT EXISTS IX_Users_Email ON Users(Email)");
    }
}