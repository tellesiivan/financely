using System.Security.Claims;
using api.data;
using api.dtos.stock;
using api.extensions;
using api.mappers;
using api.models;
using api.services.FMS;
using api.services.Stock;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace api.services.portfolio;

public class PortfolioService(UserManager<AppUser> userManager, ApplicationDbContext applicationDbContext, IStockService stockService, IFMPService fmpService):IPortfolioService
{
    public async  Task<ServiceResponse<List<models.Stock>>> GetUserPortfolio(ClaimsPrincipal user)
    {
        var response = new ServiceResponse<List<models.Stock>>();
        var userName = user.GetUsername();
        try
        {
            var appUser = await userManager.FindByNameAsync(userName);
            if (appUser is null) throw new Exception("No username found");

            var userPortfolioList =
                await applicationDbContext.Portfolios
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
            var userMatched = await userManager.FindByNameAsync(userName);
            if (userMatched is null)
            {
                throw new Exception("Unable to fulfill request at this moment");
            }

            var stockMatched = await stockService.GetStockBySymbol(symbolDto);

            if (stockMatched is null)
            {
                stockMatched = await fmpService.FindStockBySymbolAsync(symbolDto);
                if (stockMatched is null) throw new Exception("The is no results matched for the provided stock symbol");
                // save the stock to our db
                await stockService.AddStock(stockMatched.ToCreateDto());
            }

            var currentPortfolioResponse = await this.GetUserPortfolio(user);
            var portfolioList = currentPortfolioResponse.Data;

            var isExisting = portfolioList?.Any(portfolio => string.Equals(portfolio.Symbol, symbolDto.Symbol, StringComparison.CurrentCultureIgnoreCase));

            if (isExisting == true)
            {
                throw new Exception("Stock symbol has already been added");
            }

            var stockId = stockMatched.Id;
            Portfolio portfolio = new()
            {
                AppUserId = userMatched.Id,
                StockId = stockId
            };

           await applicationDbContext.Portfolios.AddAsync(portfolio);
           await applicationDbContext.SaveChangesAsync();
           
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

    public async Task<ServiceResponse<string>> DeletePortfolio(ClaimsPrincipal user, StockSymbolDto symbolDto)
    {
        var response = new ServiceResponse<string>();
        
        var username = user.GetUsername();
        var appUser = await userManager.FindByNameAsync(username);
        

        try
        {
            var userPortfolioResponse = await this.GetUserPortfolio(user);
            var userStock = userPortfolioResponse.Data;
            
            var filteredStock = userStock?.FirstOrDefault(stock =>
                stock.Symbol.ToLower() == symbolDto.Symbol.ToLower()
            );
            
            if (filteredStock is null)
            {
                throw new Exception("No matched stock with the provided symbol");
            }

            var userPortfolio = await applicationDbContext.Portfolios.FirstOrDefaultAsync(portfolio =>
                portfolio.AppUserId == appUser.Id &&
                portfolio.StockId == filteredStock.Id);


            if (userPortfolio is null)
            {
                throw new Exception("No matched portfolio");

            }

            applicationDbContext.Portfolios.Remove(userPortfolio);
           await applicationDbContext.SaveChangesAsync();

           response.IsSuccess = true;
           response.Message = "Success";
        }
        catch (Exception e)
        {
            response.IsSuccess = false;
            response.Message = e.Message;
        }

        return response;
    }


}