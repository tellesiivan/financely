using api.dtos.stock;

namespace api.services.FMS;

public interface IFMPService
{
    Task<models.Stock> FindStockBySymbolAsync(StockSymbolDto symbol);
}