using MotoVerse.Entities.Models.Motorcycles;
using MotoVerse.Infrastructure.IRepository.Motorcycles;

namespace MotoVerse.Infrastructure.Repository.Motorcycles;

public class PaymentRepository
    : GenericRepository<Payment>, IPaymentRepository
{
    public PaymentRepository(AppDbContext context)
        : base(context)
    {
    }
}