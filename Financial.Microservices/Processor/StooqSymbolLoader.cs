using Data.Loader;
using Data.Loader.Stooq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Processor
{
    public class StooqSymbolLoader
    {
        private readonly IDataLoader<StooqData> _loader;
        private readonly UnitOfWork _unitOfWork;
        private readonly Func<IList<string>> _symbolFunc;

        public StooqSymbolLoader(IDataLoader<StooqData> loader, UnitOfWork unitOfWork, Func<IList<string>> symbolFunc)
        {
            _loader = loader;
            _unitOfWork = unitOfWork;
            _symbolFunc = symbolFunc;
        }

        public async Task StoreAllSymbolsOnDatabase()
        {
            foreach (var symbol in _symbolFunc.Invoke())
            {
                try
                {
                    await StoreDataFor(symbol);
                    Console.WriteLine($"Loaded: {symbol}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }
            }
            _unitOfWork.SaveChanges();
        }

        public async Task StoreDataFor(string symbol)
        {
            var historicalDataCollection = await _loader.GetHistoricalDataFor(symbol);
            _unitOfWork.StooqRepository.InsertBulk(historicalDataCollection);
        }
    }
}
