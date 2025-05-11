using System.Threading.Tasks;
using ImageProcessingApi.Controllers;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace ImageProcessingApi.Middleware
{
    public class ApiKeyAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiKeyAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Get the requested path from the incoming HTTP request
            var path = context.Request.Path.Value;

            // Bypass static files like index.html, CSS, JS, etc.
            if (path != null && (
                    path.StartsWith("/index.html") ||
                    path.StartsWith("/css") ||
                    path.StartsWith("/js") ||
                    path.StartsWith("/images") ||
                    path.StartsWith("/favicon.ico") ||
                    path.StartsWith("/api/apikeys/generate")
                ))
            {
                await _next(context);
                return;
            }

            // Check if API key exists in headers
            if (!context.Request.Headers.TryGetValue("X-Api-Key", out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Missing API Key");
                return;
            }

            string actualApiKey = extractedApiKey;

            // Handle JSON-formatted API key
            if (extractedApiKey.ToString().Trim().StartsWith("{") && 
                extractedApiKey.ToString().Trim().EndsWith("}"))
            {
                try
                {
                    var jsonDoc = JsonDocument.Parse(extractedApiKey);
                    if (jsonDoc.RootElement.TryGetProperty("apiKey", out var apiKeyProperty))
                    {
                        actualApiKey = apiKeyProperty.GetString();
                    }
                }
                catch (JsonException)
                {
                    // If JSON parsing fails, continue with original key
                }
            }

            // Validate the API key
            if (!ApiKeysController.IsValidApiKey(actualApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid API Key");
                return;
            }

            // If the API key is valid, continue processing
            await _next(context);
        }
    }
}