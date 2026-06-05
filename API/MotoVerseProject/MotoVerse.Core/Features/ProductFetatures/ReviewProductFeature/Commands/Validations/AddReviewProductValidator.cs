using MotoVerse.Core.Features.ReviewProductFeature.Commands.Models;

namespace MotoVerse.Core.Features.ReviewProductFeature.Commands.Validations;

public class AddReviewProductValidator : AbstractValidator<AddReviewProductCommand>
{

    #region Fields 
    //private readonly 
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly IRepositoryManager _repositoryManager;

    #endregion

    #region CTOR
    public AddReviewProductValidator(IStringLocalizer<SharedResources> localizer, IRepositoryManager repositoryManager)
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
        RuleFor(x => x.Comment)
              .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                 .MaximumLength(500).WithMessage(_localizer[SharedResourcesKeys.MaxLengthis500]);

        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
            .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);
    }
    public void ApplyCustomValidationsRules()
    {

    }

    #endregion
}
