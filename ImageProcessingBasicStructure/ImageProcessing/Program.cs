using ImageProcessingApi.Middleware;
using ImageProcessingApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Registers the ImageProcessingService as a singleton for the IImageProcessingService interface.
// A singleton means one shared instance is created and used throughout the application's lifetime.
// Add services
builder.Services.AddSingleton<IImageProcessingService, ImageProcessingService>();
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registers the CacheService as a singleton for the ICacheService interface.
// This ensures consistent and centralized caching behavior across the app.
builder.Services.AddSingleton<ICacheService, CacheService>();

builder.Configuration.AddJsonFile("appsettings.json");
var app = builder.Build();

app.UseStaticFiles();

// Adds the ApiKeyAuthMiddleware to the middleware pipeline.
// This middleware will validate API keys for each incoming request before passing it to the next middleware or endpoint.
// If the API key is missing or invalid, it will return a 401 Unauthorized response.
app.UseMiddleware<ApiKeyAuthMiddleware>();

// Adds the RateLimitingMiddleware to the middleware pipeline.
// This middleware will track the number of requests made by each API key in a given time window and enforce rate limits.
// If the limit is exceeded, it will return a 429 Too Many Requests response.
app.UseMiddleware<RateLimitingMiddleware>();

app.UseRouting();
app.MapControllers();
app.Run();
