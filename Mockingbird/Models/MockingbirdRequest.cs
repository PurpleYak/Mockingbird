using System.Net.Http;

namespace Mockingbird.Models
{
    public sealed class MockingbirdRequest
    {
        public HttpMethod Method { get; set; }
        public string PathMatcherPattern { get; set; }
    }
}
