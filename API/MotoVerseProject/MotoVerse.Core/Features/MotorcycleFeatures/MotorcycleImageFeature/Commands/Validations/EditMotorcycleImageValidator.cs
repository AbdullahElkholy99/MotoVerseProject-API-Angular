using MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleImageFeature.Commands.Models;

namespace MotoVerse.Core.Features.MotorcycleFeatures.BookingFeature.Commands.Validations;

public class EditMotorcycleImageValidator
    : AbstractValidator<EditMotorcycleImageCommand>
{
    private readonly IRepositoryManager _repositoryManager;

    private readonly IStringLocalizer<SharedResources> _localizer;

    public EditMotorcycleImageValidator(
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
        RuleFor(x => x.Image)
             .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
              .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty]);

        RuleFor(x => x.MotorcycleId)
             .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);

    }


    public void ApplyCustomValidationsRules()
    {

        //RuleFor(x => x.NameEn)
        //   .MustAsync(async (model, Key, CancellationToken) => !await _repositoryManager.CategoryRepository.IsNameExistExceptSelf(x => x.NameAr == model.NameAr))
        //   .WithMessage(_localizer[SharedResourcesKeys.IsExist]);

    }
}