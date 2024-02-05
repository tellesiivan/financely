using api.dtos.stock;
using api.mappers;
using api.models;
using Newtonsoft.Json;

namespace api.services.FMS;

public class FmpService(HttpClient httpClient, IConfiguration configuration): IFMPService
{
    public async Task<models.Stock> FindStockBySymbolAsync(StockSymbolDto symbol)
    {
        try
        {
            var key = configuration["FmpKey"]!;
            var fmpResponse = await httpClient.GetAsync($"https://financialmodelingprep.com/api/v3/profile/{symbol.Symbol}?apikey={key}");
            if (!fmpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Error when trying to get information from FMP");
            }

            var content = await fmpResponse.Content.ReadAsStringAsync();
            var tasks = JsonConvert.DeserializeObject<FMPStock[]>(content);
            
            if (tasks is null)
            {
                throw new Exception("Error convert data into  json");
            }
            var fmpToStock = tasks[0].FmpToStock();
            Console.WriteLine(" ===========  fmp has results ===========");
            return fmpToStock;

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }
}