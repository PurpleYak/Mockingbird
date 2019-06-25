using System.Collections.Generic;
using System.Net;

namespace Mockingbird.Models
{
    public class MockingbirdResponse
    {
        public HttpStatusCode Status { get; set; }
        public int StatusCode => (int)Status;
        public string Body { get; set; }
        public IEnumerable<Header> Headers { get; set; }
        public int DelayInMilliseconds { get; set; }
    }
}
