using MotoVerse.Entities.Models.Motorcycles;
using MotoVerse.Infrastructure.IRepository.Motorcycles;

namespace MotoVerse.Infrastructure.Repository.Motorcycles;

public class BookingRepository
    : GenericRepository<Booking>, IBookingRepository
{
    public BookingRepository(AppDbContext context)
        : base(context)
    {
    }
}