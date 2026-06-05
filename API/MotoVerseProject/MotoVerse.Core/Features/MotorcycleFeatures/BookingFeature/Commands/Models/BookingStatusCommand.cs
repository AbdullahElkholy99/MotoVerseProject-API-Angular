namespace MotoVerse.Core.Features.MotorcycleFeatures.BookingFeature.Commands.Models;


public record ApproveBookingCommand : IRequest<Response<string>>
{
    public string Id { get; init; } = default!;
}
public record RejectBookingCommand : IRequest<Response<string>>
{
    public string Id { get; init; } = default!;
}
public record CompleteBookingCommand : IRequest<Response<string>>
{
    public string Id { get; init; } = default!;

}
public record ActiveBookingCommand : IRequest<Response<string>>
{
    public string Id { get; init; } = default!;

}
public record PayBookingCommand : IRequest<Response<string>>
{
    public string Id { get; init; } = default!;
}
public record CancelBookingCommand : IRequest<Response<string>>
{
    public string Id { get; init; } = default!;
}
public record CancelBookingByCustomerCommand : IRequest<Response<string>>
{
    public string Id { get; init; } = default!;
}
public record ReCancelBookingCommand : IRequest<Response<string>>
{
    public string Id { get; init; } = default!;
}