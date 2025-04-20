using Core.Entities;

namespace Core.Specification;

public class BookPublisherListSpecification : BaseSpecification<Product, string>
{
    public BookPublisherListSpecification()
    {
        AddSelect(x => x.BookPublisher);
        ApplyDistinct();
    }
}
