using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Mockingbird.Middleware
{
    public class EndpointResponseProcessor
    {
        public HttpResponse Response { get; }
        public EndpointResponseProcessor(HttpResponse response)
        {
            Response = response;
        }

        public void AddHeaderToResponse(string headerKey, string headerValue = "")
        {
            Response.Headers.Add(headerKey, headerValue);
        }

        public void SetStatusCode(int statusCode)
        {
            Response.StatusCode = statusCode;
        }

        public async Task SetResponseBody(string body)
        {
            await Response.WriteAsync(body);
        }

        public async Task Delay(int delayInMilliseconds)
        {
            await Task.Delay(delayInMilliseconds);
        }
    }
}
