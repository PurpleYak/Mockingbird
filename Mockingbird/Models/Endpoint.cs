using System.Collections.Generic;

namespace Mockingbird.Models
{
    public class Endpoint
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public EndpointRequest Request { get; set; }
        public EndpointResponse Response { get; set; }
        public IEnumerable<Directory> Directory { get; set; } = new List<Directory>();
        public IEnumerable<Tag> Tags { get; set; } = new List<Tag>();
        public bool Enabled { get; set; }
    }
}
