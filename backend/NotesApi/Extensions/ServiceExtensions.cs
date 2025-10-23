using NotesApi.Data;
using NotesApi.Repositories;
using NotesApi.Services;

namespace NotesApi.Extensions;

public static class ServiceExtensions
{
    public static void AddSqlServer(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton(new SqlConnectionFactory(connectionString));
        services.AddSingleton<DatabaseInitializer>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<INotesRepository, NotesRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();
    }

    public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };
            });

        services.AddAuthorization();
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<AuthService>();
        services.AddScoped<NotesService>();
    }
}
