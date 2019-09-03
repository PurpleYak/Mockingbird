using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;

namespace Mockingbird.Models
{
    public class Directory
    {
        public string Name { get; set; }
        public IEnumerable<Directory> Subdirectory { get; set; } = new List<Directory>();
        public IEnumerable<IRouter> Routes { get; set; } = new List<IRouter>();
    }
}
