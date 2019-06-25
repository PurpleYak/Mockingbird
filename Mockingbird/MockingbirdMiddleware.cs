using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mockingbird.Models;

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

        private Task<bool> HandleMockingbirdEndpointAsync(HttpContext context)
        {
            var loadedEndpoints = _configurationProvider.Load();

            return ProcessEndpoint(context, loadedEndpoints);
        }

        private async Task<bool> ProcessEndpoint(HttpContext context, IEnumerable<Models.Mockingbird> loadedEndpoints)
        {
            foreach (var endpoint in loadedEndpoints)
            {
                if (!endpoint.Enabled)
                {
                    continue;
                }

                if (endpoint.Request.Method.ToString().ToLower() != context.Request.Method.ToLower())
                {
                    continue;
                }

                var url = AppendQuery(context.Request.Path, context.Request.QueryString.Value);

                if (!Regex.IsMatch(url.ToLower(), endpoint.Request.PathMatcherPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled))
                {
                    continue;
                }

                // We match request, now sort the response
                if (_options.AddMockingbirdTestingHeaderResponse)
                {
                    context.Response.Headers.Add(_options.MockingbirdTestingHeaderResponseValue, string.Empty);
                }

                foreach (var header in endpoint.Response.Headers)
                {
                    context.Response.Headers.Add(header.Key, header.Value);
                }

                context.Response.StatusCode = endpoint.Response.StatusCode;

                if (!string.IsNullOrWhiteSpace(endpoint.Response.Body))
                {
                    await context.Response.WriteAsync(endpoint.Response.Body);
                }

                await Task.Delay(endpoint.Response.DelayInMilliseconds);

                return true;

            }
            return false;
        }

        private Task<bool> HandleMockingbirdUIAsync(HttpContext context)
        {
            var path = context.Request.Path.Value.ToLower();

            if (path == _options.MockingbirdUIPath)
            {
                // load UI
            }

            if (path == _options.MockingbirdUIPath + "/endpoints")
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
