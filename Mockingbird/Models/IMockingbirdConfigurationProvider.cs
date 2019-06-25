using System.Collections.Generic;

namespace Mockingbird.Models
{
    public interface IMockingbirdConfigurationProvider
    {
        IEnumerable<Mockingbird> Load();
        void Save(IEnumerable<Mockingbird> configuration);
    }
}
