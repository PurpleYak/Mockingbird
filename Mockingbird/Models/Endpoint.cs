using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace Mockingbird.Models
{
    public class Endpoint
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public MockingbirdRequest Request { get; set; }
        public MockingbirdResponse Response { get; set; }
        public IEnumerable<Directory> Directory { get; set; } = new List<Directory>();
        public IEnumerable<Tag> Tags { get; set; } = new List<Tag>();
        public bool Enabled { get; set; }
    }
}
