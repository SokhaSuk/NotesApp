using Dapper;

namespace NotesApi.Data;

public sealed class DatabaseInitializer
{
    private readonly SqlConnectionFactory _connectionFactory;

    public DatabaseInitializer(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task EnsureCreatedAsync()
    {
        const string createTableSql = @"
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Notes]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Notes](
        [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        [UserId] NVARCHAR(200) NOT NULL,
        [Title] NVARCHAR(200) NOT NULL,
        [Content] NVARCHAR(MAX) NULL,
        [CreatedAt] DATETIME2 NOT NULL,
        [UpdatedAt] DATETIME2 NOT NULL
    );
    CREATE INDEX IX_Notes_UserId_CreatedAt ON [dbo].[Notes] ([UserId], [CreatedAt] DESC);
END
";

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        await connection.ExecuteAsync(createTableSql);
    }
}


