namespace MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleFeature.Commands.Validations;

public class EditMotorcycleValidator : AbstractValidator<EditMotorcycleCommand>
{
    private readonly IRepositoryManager _repositoryManager;

    private readonly IStringLocalizer<SharedResources> _localizer;

    public EditMotorcycleValidator(IRepositoryManager repositoryManager, IStringLocalizer<SharedResources> localizer)
    {
        _repositoryManager = repositoryManager;
        _localizer = localizer;

        ApplyValidationRules();
        ApplyCustomValidationsRules();
    }
    public void ApplyValidationRules()
    {
        RuleFor(x => x.Brand)
              .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                 .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthis100]);

        RuleFor(x => x.Model)
             .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                 .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthis100]);

        RuleFor(x => x.OwnerId)
             .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                 .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthis100]);

        RuleFor(x => x.EngineCC)
                 .GreaterThan(0).WithMessage(_localizer[SharedResourcesKeys.GreaterThanZero]);
    }
    public void ApplyCustomValidationsRules()
    {

        RuleFor(x => x.NameAr)
           .MustAsync(async (model, Key, CancellationToken) =>
                 !await _repositoryManager.MotorcycleRepository.IsNameExistExceptSelf(x => x.NameAr == model.NameAr && x.Id != model.Id))
           .WithMessage(_localizer[SharedResourcesKeys.IsExist]);

        RuleFor(x => x.NameEn)
           .MustAsync(async (model, Key, CancellationToken) =>
                !await _repositoryManager.MotorcycleRepository.IsNameExistExceptSelf(x => x.NameEn == model.NameEn && x.Id != model.Id))
           .WithMessage(_localizer[SharedResourcesKeys.IsExist]);

    }
}