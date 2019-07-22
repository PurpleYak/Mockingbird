using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mockingbird.Extensions;
using Mockingbird.Middleware;

namespace Mockingbird
{
    public class MockingbirdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly MockingbirdOptions _options;
        private readonly IMockingbirdConfigurationProvider _configurationProvider;
        private readonly ILogger _logger;

        public MockingbirdMiddleware(RequestDelegate next, IOptions<MockingbirdOptions> options, ILoggerFactory loggerFactory)
        {
            this._next = next;
            this._options = options.Value;
            this._logger = loggerFactory.CreateLogger<MockingbirdMiddleware>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (await HandleMockingbirdUIAsync(context))
            {
                return;
            }

            if (await HandleMockingbirdEndpointAsync(context))
            {
                return;
            }

            await _next(context);
        }

        private async Task<bool> HandleMockingbirdEndpointAsync(HttpContext context)
        {
            var requestProcessor = new EndpointRequestProcessor(context.Request);
            var responseProcessor = new EndpointResponseProcessor(context.Response);
            var loadedEndpoints = _configurationProvider.Load();

            foreach (var endpoint in loadedEndpoints)
            {
                if (!requestProcessor.EndpointMatches(endpoint))
                {
                    continue;
                }

                if (_options.AddMockingbirdTestingHeaderResponse)
                {
                    responseProcessor.AddHeaderToResponse(_options.MockingbirdTestingHeaderResponseValue);
                }

                foreach (var header in endpoint.Response.Headers.OrEmptyIfNull())
                {
                    responseProcessor.AddHeaderToResponse(header.Key, header.Value);
                }

                responseProcessor.SetStatusCode(endpoint.Response.StatusCode);

                if (!string.IsNullOrWhiteSpace(endpoint.Response.Body))
                {
                    await responseProcessor.SetResponseBody(endpoint.Response.Body);
                }

                await responseProcessor.Delay(endpoint.Response.DelayInMilliseconds);

                return true;
            }
            return false;
        }

        private Task<bool> HandleMockingbirdUIAsync(HttpContext context)
        {
            var path = context.Request.Path.Value.ToLower();
            var mockingBirdUIPath = _options.MockingbirdUIPath.ToLower();

            if (path == mockingBirdUIPath)
            {
                // load UI
            }

            if (path == mockingBirdUIPath + "/endpoints")
            {
                // if get load config and return
                // if post save config and return 201
            }

            return Task.FromResult(false);


        }

        private static string AppendQuery(string path, string query)
        {
            var url = path;

            if (!string.IsNullOrEmpty(query))
            {
                url += query;
            }

            return url;
        }




    }
}
