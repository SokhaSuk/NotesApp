using Dapper;
using NotesApi.Models;
using NotesApi.Data;
using Microsoft.Data.Sqlite;

namespace NotesApi.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly DatabaseConnectionFactory _connectionFactory;

    public UsersRepository(DatabaseConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        using var connection = _connectionFactory.CreateSqliteConnection();

        var sql = "SELECT * FROM Users WHERE Username = @Username";
        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        using var connection = _connectionFactory.CreateSqliteConnection();

        var sql = "SELECT * FROM Users WHERE Id = @Id";
        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
    }

    public async Task<User> CreateAsync(User user)
    {
        using var connection = _connectionFactory.CreateSqliteConnection();

        var sql = @"
            INSERT INTO Users (Username, Email, PasswordHash, CreatedAt)
            VALUES (@Username, @Email, @PasswordHash, @CreatedAt);
            SELECT * FROM Users WHERE Id = last_insert_rowid();";

        return await connection.QuerySingleAsync<User>(sql, user);
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        using var connection = _connectionFactory.CreateSqliteConnection();

        var sql = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
        var count = await connection.ExecuteScalarAsync<int>(sql, new { Username = username });
        return count > 0;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        using var connection = _connectionFactory.CreateSqliteConnection();

        var sql = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
        var count = await connection.ExecuteScalarAsync<int>(sql, new { Email = email });
        return count > 0;
    }
}
