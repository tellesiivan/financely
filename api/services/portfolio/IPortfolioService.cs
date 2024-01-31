using System.Security.Claims;
using api.dtos.stock;
using api.models;

namespace api.services.portfolio;

public interface IPortfolioService
{
    Task<ServiceResponse<List<models.Stock>>> GetUserPortfolio(ClaimsPrincipal user);
    Task<ServiceResponse<models.Portfolio>> CreatePortfolio(ClaimsPrincipal user, StockSymbolDto symbolDto);
    Task<ServiceResponse<string>> DeletePortfolio(ClaimsPrincipal user, StockSymbolDto symbolDto);
}