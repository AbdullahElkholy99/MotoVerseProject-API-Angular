using MotoVerse.Core.Features.MotorcycleFeatures.BookingFeature.Commands.Models;
using MotoVerse.Core.Features.MotorcycleFeatures.BookingFeature.Queries.Responses;

namespace MotoVerse.Core.Mapping.Motorcycles;

public partial class BookingProfile : Profile
{
    public BookingProfile()
    {
        CreateMap<AddBookingCommand, Booking>();

        CreateMap<Booking, GetBookingListResponse>();


        CreateMap<Booking, GetBasketByIdResponse>();

        CreateMap<Booking, GetBookingPaginatedListResponse>();

        CreateMap<EditBookingCommand, Booking>();
    }
}