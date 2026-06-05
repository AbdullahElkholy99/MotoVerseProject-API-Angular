using MotoVerse.Core.Features.ReviewProductFeature.Commands.Models;

namespace MotoVerse.Core.Features.ReviewProductFeature.Commands.Validations;

public class EditReviewProductValidator : AbstractValidator<EditReviewProductCommand>
{

    #region Fields 
    //private readonly 
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly IRepositoryManager _repositoryManager;

    #endregion

    #region CTOR
    public EditReviewProductValidator(IStringLocalizer<SharedResources> localizer, IRepositoryManager repositoryManager)
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
            .MaximumLength(50).WithMessage("Max Length is 500");


        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
            .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);
    }
    public void ApplyCustomValidationsRules()
    {
        //   RuleFor(x => x.Comment)
        //.MustAsync(async (model, Key, CancellationToken) =>
        //      !await _repositoryManager.CategoryRepository.IsNameExistExceptSelf(x => x.NameAr == model.NameAr && x.Id != model.Id))
        //.WithMessage(_localizer[SharedResourcesKeys.IsExist]);


    }

    #endregion
}
