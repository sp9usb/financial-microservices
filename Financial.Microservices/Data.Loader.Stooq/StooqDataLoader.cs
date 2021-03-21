using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Data.Loader.Stooq
{
    public class StooqDataLoader : IDataLoader<StooqData>
    {
        private readonly IRestClient _client;
        private readonly Func<string, Method, IRestRequest> _requestFunc;

        public StooqDataLoader(IRestClient client, Func<string, Method, IRestRequest> requestFunc)
        {
            _client = client;
            _requestFunc = requestFunc;
        }

        public async Task<IEnumerable<StooqData>> GetHistoricalDataFor(string symbol)
        {
            var request = _requestFunc.Invoke("/q/d/l", Method.GET);
            request.AddQueryParameter("s", symbol);

            var response = await _client.ExecuteAsync(request);
            if (response.ErrorException != null)
                throw response.ErrorException;

            var lines = response.Content.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Skip(1);
            return ParseHistorical(symbol, lines).ToArray();
        }

        public async Task<StooqData> GetCurrentValueFor(string symbol)
        {
            var request = _requestFunc.Invoke("/q/l", Method.GET);
            request.AddQueryParameter("s", symbol);

            var response = await _client.ExecuteAsync(request);
            if (response.ErrorException != null)
                throw response.ErrorException;

            return ParseCurrentData(response.Content);
        }

        private IEnumerable<StooqData> ParseHistorical(string symbol, IEnumerable<string> lines)
        {
            var dataCollection = new List<StooqData>();
            foreach (var line in lines)
            {
                try
                {
                    dataCollection.Add(ParseHistoricalDataFor(symbol, line));
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }
            }
            return dataCollection;
        }

        private StooqData ParseCurrentData(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                throw new ArgumentException("empty data", nameof(data));

            var result = new StooqData()
            {
                Id = Guid.NewGuid()
            };
            var splittedData = data.Split(',', StringSplitOptions.None);
            if (splittedData.GetValue(0) != null)
                result.Symbol = splittedData[0];
            if (splittedData.GetValue(1) != null && splittedData.GetValue(2) != null)
            {
                result.Date = ParseDateTime(splittedData[1] + splittedData[2]);
            }
            if (splittedData.GetValue(3) != null)
                result.Open = float.Parse(splittedData[3]);
            if (splittedData.GetValue(4) != null)
                result.High = float.Parse(splittedData[4]);
            if (splittedData.GetValue(5) != null)
                result.Low = float.Parse(splittedData[5]);
            if (splittedData.GetValue(6) != null)
                result.Close = float.Parse(splittedData[6]);
            if (splittedData.GetValue(7) != null)
                result.Volume = float.Parse(splittedData[7]);

            return result;
        }

        private StooqData ParseHistoricalDataFor(string symbol, string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                throw new ArgumentException("empty data", nameof(data));

            var result = new StooqData()
            {
                Id = Guid.NewGuid(),
                Symbol = symbol
            };
            var splittedData = data.Split(',', StringSplitOptions.None);
            if (!(splittedData.Length == 5 || splittedData.Length == 6))
                throw new ArgumentException($"Invalid data: [{data}]", nameof(data));

            result.Date = DateTime.Parse(splittedData[0]);
            result.Open = float.Parse(splittedData[1]);
            result.High = float.Parse(splittedData[2]);
            result.Low = float.Parse(splittedData[3]);
            result.Close = float.Parse(splittedData[4]);
            if (splittedData.Length > 5)
                result.Volume = float.Parse(splittedData[5]);

            return result;
        }

        private DateTime ParseDateTime(string dateTime)
        {
            var regex = new Regex(@"(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})(?<hh>\d{2})(?<mm>\d{2})(?<ss>\d{2})");

            var match = regex.Match(dateTime);

            return DateTime.Parse($"{match.Groups["year"].Value}-{match.Groups["month"].Value}-{match.Groups["day"].Value}T{match.Groups["hh"].Value}:{match.Groups["mm"].Value}:{match.Groups["ss"].Value}");
        }
    }
}
