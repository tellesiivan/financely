using System.Security.Claims;
using api.data;
using api.dtos.comment;
using api.dtos.stock;
using api.extensions;
using api.mappers;
using api.models;
using api.services.FMS;
using api.services.Stock;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.services.comment;

public class CommentService(ApplicationDbContext applicationDbContext, UserManager<AppUser> userManager, IStockService stockService, IFMPService _fmpService) : ICommentService
{
    public async Task<ServiceResponse<List<CommentDto>>> GetAll()
    {
        var response = new ServiceResponse<List<CommentDto>>();
        try
        {
            var comments = await applicationDbContext.Comments
                .Include(comment => comment.AppUser)
                .ToListAsync();
            
            var commentDtoList = comments.Select(comment => comment.Dto()).ToList();

            if (commentDtoList is null)
            {
                throw new Exception("Unable to get comments at this time");
            }
            
            response.IsSuccess = true;
            response.Data = commentDtoList;

        }
        catch (Exception e)
        {
            response.IsSuccess = false;
            response.Message = e.Message;
        }

        return response;
    }

    public async Task<ServiceResponse<CommentDto>> GetById(int id)
    {
        var response = new ServiceResponse<CommentDto>();
        try
        {
            var matchedComment = await applicationDbContext.Comments
                .Include(comment => comment.AppUser)
                .FirstOrDefaultAsync(comment => comment.Id == id);
            
            if (matchedComment is null)
            {
                throw new Exception("No match with the id provided");
            }
            
            response.IsSuccess = true;
            response.Data = matchedComment.Dto();
        }
        catch (Exception e)
        {
            response.IsSuccess = false;
            response.Message = e.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<string>> Delete(int id)
    {
        var response = new ServiceResponse<string>();
        var matchedComment = await applicationDbContext.Comments
            .FindAsync(id);

        try
        {
            if (matchedComment is null)
            {
                throw new Exception("There is no comment with the provided id");
            }

            applicationDbContext.Comments.Remove(matchedComment);
            await applicationDbContext.SaveChangesAsync();
            
            response.IsSuccess = true;
            response.Message = "Successfully deleted";
        }
        catch (Exception e)
        {
            response.IsSuccess = false;
            response.Message = e.Message;
        }

        return response;
    }

    public async Task<ServiceResponse<CommentDto>> CreateComment(string symbol, CreateCommentDto comment, ClaimsPrincipal user)
    {
        var response = new ServiceResponse<CommentDto>();
        var username = user.GetUsername();
        var appUser = await userManager.FindByNameAsync(username);
        StockSymbolDto stockSymbolDto = new()
        {
            Symbol = symbol
        };
        
        try
        {
                   
            if (appUser is null)
            {
                throw new Exception("User does not exists");
            }
            
            var stockBySymbol = await stockService.GetStockBySymbol(stockSymbolDto);

            if (stockBySymbol is null)
            {
                stockBySymbol = await _fmpService.FindStockBySymbolAsync(stockSymbolDto);
                if (stockBySymbol is null) throw new Exception("The is no results matched for the provided stock symbol");
                // save the stock to our db
                await stockService.AddStock(stockBySymbol.ToCreateDto());
            }

            var stockId = stockBySymbol.Id;
            // save the stock to our db
            await stockService.AddStock(stockBySymbol.ToCreateDto());
            
            var commentModel = comment.ToComment(stockId);
            commentModel.AppUserId = appUser.Id;
            
            await applicationDbContext.Comments.AddAsync(commentModel);
            await applicationDbContext.SaveChangesAsync();
            
            response.Data = commentModel.Dto();
            response.IsSuccess = true;
            response.Message = "Comment was successfully added";

        }
        catch (Exception e)
        {
            response.IsSuccess = false;
            response.Message = e.Message;
        }

        return response;
    }

    public async Task<ServiceResponse<CommentDto>> UpdateComment(int commentId, UpdateCommentDto comment)
    {
        var response = new ServiceResponse<CommentDto>();
        var matchedComment = await applicationDbContext.Comments.FindAsync(commentId);
        
        try
        {
            if (matchedComment.IsNull())
            {
                throw new Exception("No comment matched with the provided id");
            }

            matchedComment!.Content = comment.Content;
            matchedComment.Title = comment.Title;

           await applicationDbContext.SaveChangesAsync();

            response.Data = matchedComment.Dto();
            response.IsSuccess = true;
            response.Message = "Successfully updated comment";
        }
        catch (Exception e)
        {
            response.IsSuccess = false;
            response.Message = e.Message;
        }

        return response;
    }
}