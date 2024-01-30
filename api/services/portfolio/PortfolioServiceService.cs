using System.Security.Claims;
using api.data;
using api.dtos.stock;
using api.extensions;
using api.models;
using api.services.Stock;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.services.portfolio;

public class PortfolioServiceService(UserManager<AppUser> userManager, ApplicationDbContext applicationDbContext, IStockService stockService):IPortfolioService
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;
    private readonly IStockService _stockService = stockService;

    public async  Task<ServiceResponse<List<models.Stock>>> GetUserPortfolio(ClaimsPrincipal user)
    {
        var response = new ServiceResponse<List<models.Stock>>();
        var userName = user.GetUsername();
        try
        {
            var appUser = await _userManager.FindByNameAsync(userName);
            if (appUser is null) throw new Exception("No username found");

            var userPortfolioList =
                await _applicationDbContext.Portfolios
                    .Where(p => p.AppUserId == appUser.Id)
                    .Select(portfolio =>new models.Stock()
                    {
                        Id = portfolio.Stock.Id,
                        Industry = portfolio.Stock.Industry,
                        Purchase = portfolio.Stock.Purchase,
                        Symbol = portfolio.Stock.Symbol,
                        CompanyName = portfolio.Stock.CompanyName,
                        LastDiv = portfolio.Stock.LastDiv,
                        MarketCap = portfolio.Stock.MarketCap, 
                    })
                    .ToListAsync();
            
            response.IsSuccess = true;
            response.Message = "Here is a list of your portfolios";
            response.Data = userPortfolioList;
        }
        catch (Exception e)
        {
            response.IsSuccess = false;
            response.Message = e.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<models.Portfolio>> CreatePortfolio(ClaimsPrincipal user, StockSymbolDto symbolDto)
    {
        var response = new ServiceResponse<models.Portfolio>();
        var userName = user.GetUsername();
        
        try
        {
            var userMatched = await _userManager.FindByNameAsync(userName);
            if (userMatched is null)
            {
                throw new Exception("Unable to fulfill request at this moment");
            }
            
            var stockMatched = await _stockService.GetStockBySymbol(symbolDto);
            if (stockMatched is null)
            {
                throw new Exception("Stock symbol provided does not match");
            }

            var currentPortfolioResponse = await this.GetUserPortfolio(user);
            var portfolioList = currentPortfolioResponse.Data;

            var isExisting = portfolioList?.Any(portfolio => string.Equals(portfolio.Symbol, symbolDto.Symbol, StringComparison.CurrentCultureIgnoreCase));

            if (isExisting == true)
            {
                throw new Exception("Stock symbol has already been added");
            }

            Portfolio portfolio = new()
            {
                AppUserId = userMatched.Id,
                StockId = stockMatched.Id
            };

           await _applicationDbContext.Portfolios.AddAsync(portfolio);
           await _applicationDbContext.SaveChangesAsync();
           
           response.IsSuccess = true;
           response.Message = "Successfully created ";
           response.Data = portfolio;
        }
        catch (Exception e)
        {
            response.IsSuccess = false;
            response.Message = e.Message;
        }

        return response;
    }
}