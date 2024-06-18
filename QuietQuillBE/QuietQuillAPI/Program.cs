using Application;
using Infrastructure;
using Presentation;
using QuietQuillBE.Extensions;
using QuietQuillBE.MiddleWare;
using Microsoft.OpenApi.Models;
using System.Net.WebSockets;
using Microsoft.AspNetCore.WebSockets;
using QuietQuillBE.Endpoints;
using MediatR;
using WebSocketMiddleware = QuietQuillBE.MiddleWare.WebSocketMiddleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "QuietQuillAPI",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("NgOrigin",
        policyBuilder =>
        {
            policyBuilder.WithOrigins("http://localhost:5173", "http://localhost:3000", "http://localhost:8080", "http://localhost:5103")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});



//WS support
builder.Services.AddWebSockets(options =>
{
options.KeepAliveInterval = TimeSpan.FromSeconds(120);
});


builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPresentation();
builder.Services.AddApplication();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}


app.UseMiddleware<ValidationExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseCors("NgOrigin");

app.UseWebSockets();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<WebSocketMiddleware>();




app.Run();