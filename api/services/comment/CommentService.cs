using api.data;
using api.dtos.comment;
using api.mappers;
using api.models;
using Microsoft.EntityFrameworkCore;

namespace api.services.comment;

public class CommentService: ICommentService
{
    private readonly ApplicationDbContext _applicationDbContext;

    public CommentService(ApplicationDbContext applicationDbContext)
    {
        this._applicationDbContext = applicationDbContext;
    }
    public async Task<ServiceResponse<List<CommentDto>>> GetAll()
    {
        var response = new ServiceResponse<List<CommentDto>>();
        try
        {
            var comments = await _applicationDbContext.Comments.ToListAsync();
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
            var matchedComment = await _applicationDbContext.Comments.FindAsync(id);
            
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

    public async Task<ServiceResponse<CommentDto>> CreateComment(int stockId, CreateCommentDto comment)
    {
        var response = new ServiceResponse<CommentDto>();
        var doesStockExists = await _applicationDbContext.Stocks
            .AnyAsync(stock => stock.Id == stockId);
        
        try
        {
            if (!doesStockExists)
            {
                throw new Exception("Stock does not exists");
            }

            var commentModel = comment.ToComment(stockId);
            
            await _applicationDbContext.Comments.AddAsync(commentModel);
            await _applicationDbContext.SaveChangesAsync();
            
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
}