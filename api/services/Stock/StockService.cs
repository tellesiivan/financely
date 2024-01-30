using api.models;
using api.data;
using api.dtos.stock;
using api.helpers;
using api.mappers;
using Microsoft.EntityFrameworkCore;

namespace api.services.Stock;

public class StockService: IStockService
{
    private readonly ApplicationDbContext _applicationDbContext;

    public StockService(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<ServiceResponse<List<StockDto>>> GetAllStocks(QueryObject queryObject)
    {
        var response = new ServiceResponse<List<StockDto>>();

        try
        {
            // AsQueryable: This is a convenience method to help with disambiguation of extension methods in the same namespace that extend both interfaces. 
            // Pretty much allows you to make additional queries before making the results into a list
            var queryableStocks = _applicationDbContext.Stocks
                .Include(s => s.Comments)
                .AsQueryable();

            if (!string.IsNullOrEmpty(queryObject.CompanyName))
            {
                queryableStocks = queryableStocks.Where(stock =>
                    stock.CompanyName.Contains(queryObject.CompanyName));
                
            } else if (!string.IsNullOrEmpty(queryObject.Symbol))
            {
                queryableStocks = queryableStocks.Where(stock =>
                    stock.CompanyName.Contains(queryObject.Symbol));
            } else if (!string.IsNullOrEmpty(queryObject.SortBy))
            {
                // OrdinalIgnoreCase: Compare strings using ordinal (binary) sort rules and ignoring the case of the strings being compared.
                if (queryObject.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    queryableStocks = queryObject.IsDescending
                        ? queryableStocks.OrderByDescending(stock => stock.Symbol)
                            // OrderBy: Sorts the elements of a sequence in ascending order according to a key.
                        : queryableStocks.OrderBy(stock => stock.Symbol);
                }
            }

            // example: queryObject.PageNumber = 4,
            // queryObject.PageSize = 50
            // 4 - 1(starts at 0 so subtract 1, -1 -> prevents the PageNumber to be one ahead) = 3
            // 3 * 50 = 150, we skip the first 150 items and take(get) the next 50(PageSize) items
            var skipNumber = (queryObject.PageNumber - 1) * queryObject.PageSize;
                // skip the first X number of items and Take(get) Y number of items
            var stockList = await queryableStocks.Skip(skipNumber).Take(queryObject.PageSize).ToListAsync();
            
            var mappedStockList = stockList.Select(stock => stock.ToStockDto())
                .ToList();
            
            if (mappedStockList is null)
            {
                throw new Exception("No stocks found");
            }

            response.Data = mappedStockList;
            response.Message = "Here is your list of stocks";
            response.IsSuccess = true;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.IsSuccess = false;
        }


        return response;
    }

    public async Task<ServiceResponse<StockDto>> GetStock(int id)
    {
        var response = new ServiceResponse<StockDto>();
        
        try
        {
            // .Find(Finds an entity with the given primary key values)
            var matchedStock =
                await _applicationDbContext.Stocks
                    .Include(s => s.Comments)
                    .FirstOrDefaultAsync(s => s.Id == id);

            if (matchedStock is null)
            {
                throw new Exception("There is no stock with the id provided");
            }

            response.IsSuccess = true;
            response.Data = matchedStock.ToStockDto();
            response.Message = "success";
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.IsSuccess = false;
        }

        return response;
    }

    public async Task<ServiceResponse<models.Stock>> UpdateStock(int id, UpdateStockRequestDto stockRequestDto)
    {
        var response = new ServiceResponse<models.Stock>();

        try
        {
            var matchedStock = await _applicationDbContext.Stocks.FindAsync(id);

            if (matchedStock is null)
            {
                throw new Exception("There was no stock matched with the provided id");
            }

            matchedStock.CompanyName = stockRequestDto.CompanyName;
            matchedStock.MarketCap = stockRequestDto.MarketCap;
            matchedStock.Industry = stockRequestDto.Industry;
            matchedStock.LastDiv = stockRequestDto.LastDiv;
            matchedStock.Purchase = stockRequestDto.Purchase;
            matchedStock.Symbol = stockRequestDto.Symbol;

            await _applicationDbContext.SaveChangesAsync();
            
            response.Data = matchedStock;
            response.Message = "success";
            response.IsSuccess = true;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.IsSuccess = false;
        }

        return response;
    }

    public async Task<ServiceResponse<StockDto>> AddStock(CreateStockRequestDto stockRequest)
    {
        var response = new ServiceResponse<StockDto>();
        var stockModel = stockRequest.ToStockFromCreateDto();
        try
        {
             await _applicationDbContext.Stocks.AddAsync(stockModel);
             await _applicationDbContext.SaveChangesAsync();
             var matchResponse = await this.GetStock(stockModel.Id);

             if (matchResponse?.Data is null)
             {
                 throw new Exception("matchResponse error");
             }
             
             response.IsSuccess = true;
             response.Data = matchResponse.Data;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.IsSuccess = false;
        }
        return response;
    }

    public async Task<bool> DoesExists(int id)
    {
        return await _applicationDbContext.Stocks.AnyAsync(stock => stock.Id == id);
    }

    public async Task<models.Stock?> GetStockBySymbol(StockSymbolDto stockSymbolDto)
    {
        return await _applicationDbContext.Stocks.FirstOrDefaultAsync(stock => stock.Symbol.ToLower() == stockSymbolDto.Symbol.ToLower());
    }
}