using MotoVerse.Core.Features.ReviewProductFeature.Commands.Models;
using MotoVerse.Core.Features.ReviewProductFeature.Queries.Responses;

namespace MotoVerse.Core.Mapping.Products;

public class ReviewProductProfile : Profile
{
    public ReviewProductProfile()
    {

        CreateMap<ReviewProduct, GetReviewProductListResponse>();

        CreateMap<ReviewProduct, GetReviewProductPaginatedListResponse>();
        CreateMap<AddReviewProductCommand, ReviewProduct>();
        CreateMap<EditReviewProductCommand, ReviewProduct>();
    }

}
