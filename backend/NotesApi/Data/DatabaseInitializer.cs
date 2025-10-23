using Dapper;
using System.Data.SqlClient;

namespace NotesApi.Data;

public class DatabaseInitializer
{
    private readonly SqlConnectionFactory _connectionFactory;

    public DatabaseInitializer(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task InitializeAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        await connection.ExecuteAsync(@"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
            CREATE TABLE Users (
                Id INT IDENTITY(1,1) PRIMARY KEY,
                Username NVARCHAR(50) NOT NULL UNIQUE,
                Email NVARCHAR(100) NOT NULL UNIQUE,
                PasswordHash NVARCHAR(255) NOT NULL,
                CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
            )");

        await connection.ExecuteAsync(@"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Notes' AND xtype='U')
            CREATE TABLE Notes (
                Id INT IDENTITY(1,1) PRIMARY KEY,
                Title NVARCHAR(200) NOT NULL,
                Content NVARCHAR(MAX),
                CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
                UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
                UserId INT NOT NULL,
                FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
            )");
    }
}