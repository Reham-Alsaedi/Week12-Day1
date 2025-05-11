using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;

namespace ImageProcessingApi.Controllers
{
    [ApiController]
    [Route("api/apikeys")]
    public class ApiKeysController : ControllerBase
    {
        private static readonly ConcurrentDictionary<string, bool> ApiKeys = new();

        [HttpPost("generate")]
        public IActionResult GenerateApiKey()
        {
            var apiKey = Guid.NewGuid().ToString();
            ApiKeys[apiKey] = true;
            return Ok(new { apiKey });
        }

        public static bool IsValidApiKey(string apiKey) => ApiKeys.ContainsKey(apiKey);
    }
}
