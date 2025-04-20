using Core.Entities;

namespace Core.Specification;

public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(ProductSPecParams specParams) : base(x =>
        (string.IsNullOrEmpty(specParams.Search) || x.Name.ToLower().Contains(specParams.Search)) &&
        (specParams.BookPublishers.Count == 0 || specParams.BookPublishers.Contains(x.BookPublisher)) &&
        (specParams.Types.Count == 0 || specParams.Types.Contains(x.Type))
    )
    {

        ApplyPaging(specParams.PageSize * (specParams.PageIndex -1), specParams.PageSize);

        switch (specParams.Sort)
        {
            case "priceAsc":
                AddOrderBy(x => x.Price);
                break;
            case "priceDesc":
                AddOrderByDescending(x => x.Price);
                break;
            default:
                AddOrderBy(x => x.Name);
                break;
        }
    }
}
