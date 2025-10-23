using Dapper;
using NotesApi.Models;
using System.Data.SqlClient;

namespace NotesApi.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly SqlConnectionFactory _connectionFactory;

    public UsersRepository(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = "SELECT * FROM Users WHERE Username = @Username";
        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = "SELECT * FROM Users WHERE Id = @Id";
        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
    }

    public async Task<User> CreateAsync(User user)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = @"
            INSERT INTO Users (Username, Email, PasswordHash, CreatedAt)
            OUTPUT INSERTED.*
            VALUES (@Username, @Email, @PasswordHash, @CreatedAt)";

        return await connection.QuerySingleAsync<User>(sql, user);
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
        var count = await connection.ExecuteScalarAsync<int>(sql, new { Username = username });
        return count > 0;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
        var count = await connection.ExecuteScalarAsync<int>(sql, new { Email = email });
        return count > 0;
    }
}
