using System.Collections.Generic;

namespace Mockingbird.Models
{
    public interface IMockingbirdConfigurationProvider
    {
        IEnumerable<Endpoint> Load();
        void Save(IEnumerable<Endpoint> configuration);
    }
}
