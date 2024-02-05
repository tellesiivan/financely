namespace api.models;

public class CommentQuery
{
    public string Symbol { get; set; } = string.Empty;
    public bool IsDescending { get; set; } = true;
}