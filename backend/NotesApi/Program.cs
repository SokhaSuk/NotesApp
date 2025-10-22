using Dapper;
using Microsoft.Data.SqlClient;
using NotesApi.Data;
using NotesApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
const string CorsPolicyName = "DefaultCors";
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? new[] { "http://localhost:5173" };
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyName, policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// Dapper defaults
DefaultTypeMap.MatchNamesWithUnderscores = true;

// Data services
builder.Services.AddSingleton(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection")
        ?? "Server=localhost;Database=NotesApp;Trusted_Connection=True;TrustServerCertificate=True;";
    return new SqlConnectionFactory(connectionString);
});
builder.Services.AddSingleton<DatabaseInitializer>();

// Repositories
builder.Services.AddScoped<INotesRepository, NotesRepository>();

var app = builder.Build();

app.UseCors(CorsPolicyName);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

// Ensure database/table exist
await app.Services.GetRequiredService<DatabaseInitializer>().EnsureCreatedAsync();

app.Run();


