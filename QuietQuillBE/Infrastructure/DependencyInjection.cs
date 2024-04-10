using Application.Abstraction.Authentication;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        
        

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
