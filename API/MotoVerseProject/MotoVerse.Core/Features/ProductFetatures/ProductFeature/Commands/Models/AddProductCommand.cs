namespace MotoVerse.Core.Features.ProductFeature.Commands.Models;

public class AddProductCommand : IRequest<Response<GetProductByIdResponse>>
{
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public int Quantity { get; set; }
    public int Price { get; set; }
    public IFormFile? ImageFile { get; set; }
    public string CategoryId { get; set; }
    public string AdminId { get; set; }

    public string Description { get; set; } = string.Empty;
    public double? Rating { get; set; }

    public ProductStatus Status { get; set; } = ProductStatus.Active;

}
