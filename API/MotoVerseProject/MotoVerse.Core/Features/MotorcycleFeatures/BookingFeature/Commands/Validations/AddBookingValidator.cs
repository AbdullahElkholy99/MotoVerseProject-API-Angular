using MotoVerse.Core.Features.MotorcycleFeatures.BookingFeature.Commands.Models;
using MotoVerse.Entities.Enums.Motorcycles;

namespace MotoVerse.Core.Features.MotorcycleFeatures.BookingFeature.Commands.Validations;

public class AddBookingValidator
    : AbstractValidator<AddBookingCommand>
{
    private readonly IRepositoryManager _repositoryManager;

    private readonly IStringLocalizer<SharedResources> _localizer;

    public AddBookingValidator(
        IRepositoryManager repositoryManager,
        IStringLocalizer<SharedResources> localizer)
    {
        _repositoryManager = repositoryManager;
        _localizer = localizer;

        ApplyValidationRules();
        ApplyCustomValidationsRules();
    }
    public void ApplyValidationRules()
    {
        //RuleFor(x => x.StartDate)
        //      .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty]);
        //RuleFor(x => x.EndDate)
        //      .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty]);

        //RuleFor(x => x.MotorcycleId)
        //     .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
        //         .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);

        //RuleFor(x => x.CustomerId)
        //     .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
        //         .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);

        //RuleFor(x => x.TotalPrice)
        //         .GreaterThan(0).WithMessage(_localizer[SharedResourcesKeys.GreaterThanZero]);
        //RuleFor(x => x.EndDate)
        //         .GreaterThan(x => x.StartDate).WithMessage(_localizer[SharedResourcesKeys.GreaterThanStartDate]);

        RuleFor(x => x.MotorcycleId)
         .NotEmpty();

        RuleFor(x => x.PickupLocation)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(x => x.StartDate)
            .NotEmpty();

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be after start date.");

        RuleFor(x => x.PaymentMethod)
            .IsInEnum();

        When(x => x.PaymentMethod == PaymentMethod.Card, () =>
        {
            RuleFor(x => x.Provider)
                .NotEqual(PaymentProvider.None);
        });

        When(x => x.Provider == PaymentProvider.Paypal, () =>
        {
            RuleFor(x => x.PaypalEmail)
                .NotEmpty()
                .EmailAddress();
        });

        When(x => x.PaymentMethod == PaymentMethod.Wallet, () =>
        {
            RuleFor(x => x.WalletName)
                .NotEmpty();

            RuleFor(x => x.WalletPhone)
                .NotEmpty()
                .Matches(@"^01[0-9]{9}$");
        });
    }



    public void ApplyCustomValidationsRules()
    {

        //RuleFor(x => x.NameEn)
        //   .MustAsync(async (model, Key, CancellationToken) => !await _repositoryManager.CategoryRepository.IsNameExistExceptSelf(x => x.NameAr == model.NameAr))
        //   .WithMessage(_localizer[SharedResourcesKeys.IsExist]);

    }
}
