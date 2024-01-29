using System.Security.Claims;
using api.data;
using api.extensions;
using api.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.services.portfolio;

public class Portfolio(UserManager<AppUser> userManager, ApplicationDbContext applicationDbContext):IPortfolio
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

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
}