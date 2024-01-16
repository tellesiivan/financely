using api.models;

namespace api.extensions;

public static class CommentExtensions
{
    public static bool IsNull(this Comment? comment) => comment is null;
}