using api.dtos.stock;
using api.models;
using api.services.portfolio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.controllers;

[Route("[controller]")]
[ApiController]
public class PortfolioController(IPortfolioService portfolioService): ControllerBase
{
    private readonly IPortfolioService _portfolioService = portfolioService;

    [HttpGet("all")]
    [Authorize]
    public async Task<ActionResult<ServiceResponse<StockDto>>> GetUserPortfolio()
    {
        var res = await _portfolioService.GetUserPortfolio(User);
        return res.IsSuccess ? Ok(res) : BadRequest(res);
    }

    [HttpPost("create")]
    [Authorize]
    public async Task<ActionResult<ServiceResponse<Portfolio>>> CreatePortfolio([FromBody] StockSymbolDto symbolDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _portfolioService.CreatePortfolio(User, symbolDto);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }
    
}