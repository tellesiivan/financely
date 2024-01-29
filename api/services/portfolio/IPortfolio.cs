using System.Security.Claims;
using api.dtos.stock;
using api.models;

namespace api.services.portfolio;

public interface IPortfolio
{
    Task<ServiceResponse<List<models.Stock>>> GetUserPortfolio(ClaimsPrincipal user);
}