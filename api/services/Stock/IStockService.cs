namespace api.services.Stock;

public interface IStockService
{
    Task<List<models.Stock>> GetAllStocks();
}