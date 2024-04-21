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
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(
                configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(10, 4, 32))
            ));
        services.AddScoped<IApplicationDbContext>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IUnitOfWork>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());
        
        services.AddScoped<IDbConnection>(sp =>
        {
            var configuration = sp.GetService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            return new MySqlConnection(connectionString); // Assuming you are using MySQL as per your DbContext options.
        });
        
        
        
        services.AddScoped<IAdminRepository, AdminRepository>();
        services.AddScoped<IHabitLogRepository, HabitLogRepository>();
        services.AddScoped<IHabitRepository, HabitRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IJournalEntryRepository, JournalEntryRepository>();
        services.AddScoped<IHabitLogRepository, HabitLogRepository>();
        services.AddScoped<IMeditationSessionRepository, MeditationSessionRepository>();
        services.AddScoped<IMoodRepository, MoodRepository>();
        services.AddScoped<IQuizQuestionRepository, QuizQuestionRepository>();
        services.AddScoped<IQuizRepository, QuizRepository>();
        services.AddScoped<IReminderRepository, ReminderRepository>();
        services.AddScoped<IReminderRepository, ReminderRepository>();
        services.AddScoped<IUserMeditationRecordRepository, UserMeditationRecordRepository>();
        services.AddScoped<IUserQuizRecordRepository, UserQuizRecordRepository>();
        
        
        
        
        
        
        
        
        
        FirebaseApp.Create(new AppOptions
        {
            Credential = GoogleCredential.FromFile("firebase.json")
        });
        services.AddSingleton<IAuthenticationService, AuthenticationService>();
        
        services.AddHttpClient<IJWTProvider, IJWTProvider>((sp ,httpClient) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            
            httpClient.BaseAddress = new Uri(config["Authentication:TokenUri"]);
        });

        services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtOptions =>
        {
            jwtOptions.Audience = configuration["Authentication:Audience"];
            jwtOptions.TokenValidationParameters.ValidIssuer = configuration["Authentication:Issuer"];
            
            jwtOptions.Authority = "https://securetoken.google.com/your-project-id";
            jwtOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = "https://securetoken.google.com/your-project-id",
                ValidateAudience = true,
                ValidAudience = "your-project-id",
                ValidateLifetime = true
            };
        });
        
        return services;
    }
}
