namespace Core.Specification;

public class ProductSPecParams
{
    private const int MaxPageSize = 25;
    public int PageIndex { get; set; } = 1;

    private int _pageSize = 6;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
    

    private List<string> _bookPublishers = [];
    public List<string> BookPublishers
    {
        get => _bookPublishers;
        set
        {
            _bookPublishers = value.SelectMany(x => x.Split(',', 
                StringSplitOptions.RemoveEmptyEntries)).ToList();
        }
    }

    private List<string> _types = [];
    public List<string> Types
    {
        get => _types;
        set
        {
            _types = value.SelectMany(x => x.Split(',', 
                StringSplitOptions.RemoveEmptyEntries)).ToList();
        }
    }

    public string? Sort { get; set; }

    private string? _search;
    public string Search
    {
        get => _search ?? "";
        set => _search = value.ToLower();
    }
    
    
}
