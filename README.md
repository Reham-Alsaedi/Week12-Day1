# Rate-Limited Image Processing API with Caching

This project is a RESTful API built with .NET Core that allows users to upload images and apply basic filters such as grayscale and sepia. It includes rate limiting, caching for performance, and API key-based authentication.

---

## Features

### Image Upload and Processing

**Endpoint:** `POST /api/images/process`

- Accepts image files via `multipart/form-data`.
- Requires a valid API key in the request headers.
- Applies filters specified via query parameters or request body (e.g., `filter=grayscale` or `filter=sepia`).
- Rate limiting middleware tracks requests per API key and returns `429 Too Many Requests` if limits are exceeded.
- Caching middleware checks for previously processed images to avoid redundant processing.
- Returns the processed image with the appropriate `Content-Type`.

### API Key Management

**Endpoint:** `POST /api/apikeys/generate`

- Generates and stores a unique API key.
- Returns the API key to the client.
- API keys can be stored in memory or in a configuration file.

---

## Technical Details

- Built with ASP.NET Core and C#.
- Custom middleware for:
  - API key authentication
  - Rate limiting
- Basic image manipulation (grayscale, sepia) using a simple image processing utility.
- Caching implemented using in-memory dictionaries or simple file-based storage.
- Configuration (e.g., rate limits, API key list) can be managed via `appsettings.json`.
- Error handling for:
  - Invalid or missing API keys
  - Exceeding rate limits
  - Invalid image uploads or unsupported formats

---

## Project Structure

ProjectRoot/
│
├── Controllers/
│ ├── ImageController.cs
│ └── ApiKeyController.cs
│
├── Middleware/
│ ├── ApiKeyMiddleware.cs
│ └── RateLimitingMiddleware.cs
│
├── Services/
│ ├── CacheService.cs
│ └── ImageProcessor.cs
│
├── Models/
│ └── ApiKeyStore.cs
│
└── appsettings.json

## Getting Started

1. Clone the repository.
2. Configure your `appsettings.json` with rate limit and API key settings.
3. Run the application using `dotnet run`.
4. Use tools like Postman to:
   - Generate an API key.
   - Upload and process images.
