namespace Core.Entities.OrderAggregate;

public class ProductItemOrdered
{
    public int ProductId { get; set; }
    public required string ProdudctName { get; set; }
    public required string PictureUrl { get; set; }

}
