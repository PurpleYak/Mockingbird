using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace Mockingbird.Middleware
{
    public class EndpointRequestProcessor
    {
        private readonly HttpRequest _request;
        public EndpointRequestProcessor(HttpRequest request)
        {
            this._request = request;
        }

        public bool EndpointMatches(Models.Endpoint endpoint)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException("Endpoint cannot be null");
            }

            if (!endpoint.Enabled)
            {
                return false;
            }

            if (endpoint.Request.Method.ToString().ToLower() != _request.Method.ToLower())
            {
                return false;
            }

            var url = AppendQuery(_request.Path.Value, _request.QueryString.Value);

            if (!Regex.IsMatch(url, endpoint.Request.PathMatcherPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled))
            {
                return false;
            }

            return true;
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
