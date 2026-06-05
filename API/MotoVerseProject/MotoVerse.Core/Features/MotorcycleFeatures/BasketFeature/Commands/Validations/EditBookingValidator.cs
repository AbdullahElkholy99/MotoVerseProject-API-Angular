//using MotoVerse.Core.Features.MotorcycleFeatures.BookingFeature.Commands.Models;

//namespace MotoVerse.Core.Features.MotorcycleFeatures.BookingFeature.Commands.Validations;

//public class EditBookingValidator
//    : AbstractValidator<EditBookingCommand>
//{
//    private readonly IRepositoryManager _repositoryManager;

//    private readonly IStringLocalizer<SharedResources> _localizer;

//    public EditBookingValidator(
//        IRepositoryManager repositoryManager,
//        IStringLocalizer<SharedResources> localizer)
//    {
//        _repositoryManager = repositoryManager;
//        _localizer = localizer;

//        ApplyValidationRules();
//        ApplyCustomValidationsRules();
//    }
//    public void ApplyValidationRules()
//    {
//        RuleFor(x => x.StartDate)
//              .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty]);
//        RuleFor(x => x.EndDate)
//              .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty]);

//        RuleFor(x => x.MotorcycleId)
//             .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
//                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);

//        RuleFor(x => x.CustomerId)
//             .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
//                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);

//        RuleFor(x => x.TotalPrice)
//                 .GreaterThan(0).WithMessage(_localizer[SharedResourcesKeys.GreaterThanZero]);
//        RuleFor(x => x.EndDate)
//                 .GreaterThan(x => x.StartDate).WithMessage(_localizer[SharedResourcesKeys.GreaterThanStartDate]);
//    }


//    public void ApplyCustomValidationsRules()
//    {

//        //RuleFor(x => x.NameEn)
//        //   .MustAsync(async (model, Key, CancellationToken) => !await _repositoryManager.CategoryRepository.IsNameExistExceptSelf(x => x.NameAr == model.NameAr))
//        //   .WithMessage(_localizer[SharedResourcesKeys.IsExist]);

//    }
//}