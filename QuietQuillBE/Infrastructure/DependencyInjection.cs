using System.Data;
using Application.Abstraction.Authentication;
using Domain.Abstraction;
using Domain.Repositories;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Infrastructure.Authentication;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string primaryConnectionString = configuration.GetConnectionString("DefaultConnection");
        string fallbackConnectionString = configuration.GetConnectionString("DockerConnection");

        // Test primary database connection
        bool primaryConnectionAvailable = TestConnection(primaryConnectionString);
        string connectionString = primaryConnectionAvailable ? primaryConnectionString : fallbackConnectionString;
        //string connectionString = primaryConnectionString;
        Version primaryVersion = new(10, 4, 32); 
        Version fallbackVersion = new(8, 0, 21); 
        Version serverVersion = primaryConnectionAvailable ? primaryVersion : fallbackVersion;
        //Version serverVersion = primaryVersion;
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(
                connectionString,
                new MySqlServerVersion(serverVersion),
                mySqlOptions => mySqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null)));

        services.AddScoped<IApplicationDbContext>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IUnitOfWork>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IDbConnection>(sp => new MySqlConnection(connectionString));

        services.AddScoped<IAdminRepository, AdminRepository>();
        services.AddScoped<IHabitLogRepository, HabitLogRepository>();
        services.AddScoped<IHabitRepository, HabitRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IJournalEntryRepository, JournalEntryRepository>();
        services.AddScoped<IMeditationSessionRepository, MeditationSessionRepository>();
        services.AddScoped<IMoodRepository, MoodRepository>();
        services.AddScoped<IQuizQuestionRepository, QuizQuestionRepository>();
        services.AddScoped<IQuizRepository, QuizRepository>();
        services.AddScoped<IReminderRepository, ReminderRepository>();
        services.AddScoped<IUserMeditationRecordRepository, UserMeditationRecordRepository>();
        services.AddScoped<IUserQuizRecordRepository, UserQuizRecordRepository>();

        // Firebase Setup
        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile("firebase.json")
            });
        }
        services.AddSingleton<IAuthenticationService, AuthenticationService>();

        services.AddHttpClient<IJWTProvider, JWTProvider>((sp, httpClient) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            httpClient.BaseAddress = new Uri(config["Authentication:TokenUri"]);
        });

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(jwtOptions =>
            {
                jwtOptions.Authority = configuration["Authentication:ValidIssuer"];
                jwtOptions.Audience = configuration["Authentication:Audience"];
                jwtOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["Authentication:ValidIssuer"],
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true
                };
            });
        
        services.AddAuthorization();

        return services;
    }

    private static bool TestConnection(string connectionString)
    {
        try
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                return true;
            }
        }
        catch
        {
            return false;
        }
    }
}