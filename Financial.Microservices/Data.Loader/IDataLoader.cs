using System.Collections.Generic;

namespace Data.Loader
{
    public interface IDataLoader<out T>
    {
        IEnumerable<T> Load();
    }
}
