using Microsoft.Data.Sqlite;

namespace NotesApi.Data;

public class DatabaseConnectionFactory
{
    private readonly string _connectionString;

    public DatabaseConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqliteConnection CreateSqliteConnection()
    {
        return new SqliteConnection(_connectionString);
    }
}