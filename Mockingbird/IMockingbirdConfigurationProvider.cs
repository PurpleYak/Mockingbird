using System.Collections.Generic;
using Mockingbird.Models;

namespace Mockingbird
{
    public interface IMockingbirdConfigurationProvider
    {
        IEnumerable<Endpoint> Load();
        void Save(IEnumerable<Endpoint> configuration);
    }
}
