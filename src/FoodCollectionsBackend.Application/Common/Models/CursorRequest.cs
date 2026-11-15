namespace FoodCollectionsBackend.Application.Common.Models;

public class CursorRequest
{
    private const int MaxPageSize = 100;
    private int _pageSize = 20;

    public string? Cursor { get; set; }
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
    public string? SearchString { get; set; }
}
