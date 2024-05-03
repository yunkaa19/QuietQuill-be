using Application;
using Application.Abstractions.Links;
using Infrastructure;
using Presentation;
using QuietQuillBE.Extensions;
using QuietQuillBE.MiddleWare;


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
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPresentation();
builder.Services.AddApplication();

builder.Services.AddAuthorization();






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


app.MapControllers();
app.UseAuthorization();

app.Run();
