using Data.Loader.Stooq;
using LiteDB;
using System;
using System.IO;

namespace Processor
{
    public class UnitOfWork : IDisposable
    {
        private readonly Stream _databaseStream;
        private readonly LiteDatabase _databaseContext;
        public ILiteCollection<StooqData> StooqRepository
        {
            get
            {
                return _databaseContext.GetCollection<StooqData>("stooq_data", BsonAutoId.Guid);
            }
        }

        public UnitOfWork(string databasePath)
        {
            _databaseStream = File.Open(databasePath, FileMode.OpenOrCreate);
            _databaseContext = new LiteDatabase(_databaseStream);
        }

        public void SaveChanges()
        {
            _databaseContext.Commit();
        }

        public void Dispose()
        {
            _databaseContext?.Dispose();
            ((IDisposable)_databaseStream).Dispose();
        }
    }
}
