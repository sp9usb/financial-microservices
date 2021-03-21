using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Loader
{
    public interface IDataLoader<T>
    {
        Task<IEnumerable<T>> GetHistoricalDataFor(string symbol);
        Task<T> GetCurrentValueFor(string symbol);
    }
}
