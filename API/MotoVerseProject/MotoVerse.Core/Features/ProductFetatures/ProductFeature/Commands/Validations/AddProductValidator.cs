using MotoVerse.Infrastructure.IRepository.Base;

namespace MotoVerse.Core.Features.ProductFeature.Commands.Validations;

public class AddProductValidator : AbstractValidator<AddProductCommand>
{

    #region Fields 
    //private readonly 
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly IRepositoryManager _repositoryManager;

    #endregion

    #region CTOR
    public AddProductValidator(IStringLocalizer<SharedResources> localizer, IRepositoryManager repositoryManager)
    {
        _localizer = localizer;
        _repositoryManager = repositoryManager;
        ApplyValidationsRules();
        ApplyCustomValidationsRules();
    }
    #endregion

    #region Actions
    public void ApplyValidationsRules()
    {
        RuleFor(x => x.NameAr)
              .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                 .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthis100]);

        RuleFor(x => x.NameEn)
             .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                 .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthis100]);

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
            .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
            .GreaterThan(0).WithMessage("Product Price Must Greater Than 0");

        RuleFor(x => x.Quantity)
            .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
            .GreaterThan(0).WithMessage("Product Quantity Must Greater Than 0");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
            .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);
    }
    public void ApplyCustomValidationsRules()
    {

        RuleFor(x => x.NameAr)
               .MustAsync(async (model, Key, CancellationToken) =>
               !await _repositoryManager.ProductRepository.IsNameExistExceptSelf(x => x.NameAr == model.NameAr))
               .WithMessage(_localizer[SharedResourcesKeys.IsExist]);

        RuleFor(x => x.NameEn)
           .MustAsync(async (model, Key, CancellationToken) =>
           !await _repositoryManager.ProductRepository.IsNameExistExceptSelf(x => x.NameEn == model.NameEn))
           .WithMessage(_localizer[SharedResourcesKeys.IsExist]);

    }

    #endregion
}
