using api.dtos.stock;
using api.models;
using api.services.portfolio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.controllers;

[Route("[controller]")]
[ApiController]
public class PortfolioController(IPortfolio portfolio): ControllerBase
{
    private readonly IPortfolio _portfolio = portfolio;

    [HttpGet("all")]
    [Authorize]
    public async Task<ActionResult<ServiceResponse<StockDto>>> GetUserPortfolio()
    {
        var res = await _portfolio.GetUserPortfolio(User);
        return res.IsSuccess ? Ok(res) : BadRequest(res);
    }
}