using api.models;
using api.data;
using Microsoft.EntityFrameworkCore;

namespace api.services.Stock;

public class StockService: IStockService
{
    private readonly ApplicationDbContext _applicationDbContext;

    public StockService(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }


    public async Task<List<models.Stock>> GetAllStocks()
    {
        return await _applicationDbContext.Stocks.ToListAsync();
    }
}