using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Mockingbird
{
    public class MockingbirdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly MockingbirdOptions _options;
        private readonly ILogger _logger;

        public MockingbirdMiddleware(RequestDelegate next, IOptions<MockingbirdOptions> options, ILoggerFactory loggerFactory)
        {
            this._next = next;
            this._options = options.Value;
            this._logger = loggerFactory.CreateLogger<MockingbirdMiddleware>();



        }

        public async Task InvokeAsync(HttpContext context)
        {
        }
    }
}
