using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Tracker.Infrastructure;
using Tracker.Services;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddServices();

// CORS (allow Vercel frontend + local development)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "https://intership-project-2026-la9p.vercel.app",
                "https://intership-project-2026.vercel.app",
                "http://localhost:3000",
                "http://localhost:5173"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "dev-secret")
            ),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

var app = builder.Build();

// Seed database with sample anime
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<Tracker.Infrastructure.Data.TrackerDbContext>();
    await Tracker.API.Data.DbSeeder.SeedSampleDataAsync(context);
}

// Swagger ENABLED in Production
app.UseSwagger();
app.UseSwaggerUI();

// Root endpoint
app.UseCors("AllowFrontend");

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();
