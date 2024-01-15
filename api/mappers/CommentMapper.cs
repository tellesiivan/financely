using api.dtos.comment;
using api.models;

namespace api.mappers;

public static class CommentMapper
{
    public static CommentDto Dto(this Comment comment)
    {
        return new CommentDto()
        {
            StockId = comment.StockId,
            Content = comment.Content,
            Id = comment.Id,
            Title = comment.Title,
            CreatedOn = comment.CreatedOn
        };
    }
    public static Comment ToComment(this CreateCommentDto comment, int stockId)
    {
        return new Comment()
        {
         Content = comment.Content,
         Title = comment.Title,
         StockId = stockId,
        };
    }
}