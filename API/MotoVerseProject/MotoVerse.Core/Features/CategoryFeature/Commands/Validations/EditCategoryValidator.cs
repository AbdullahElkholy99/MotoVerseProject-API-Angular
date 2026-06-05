using MotoVerse.Core.Features.CategoryFeature.Commands.Models;
using MotoVerse.Infrastructure.IRepository.Base;

namespace MotoVerse.Core.Features.CategoryFeature.Commands.Validations;

public class EditCategoryValidator : AbstractValidator<EditCategoryCommand>
{

    #region Fields 
    //private readonly 
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly IRepositoryManager _repositoryManager;

    #endregion

    #region CTOR
    public EditCategoryValidator(IStringLocalizer<SharedResources> localizer, IRepositoryManager repositoryManager)
    {
        _localizer = localizer;
        _repositoryManager = repositoryManager;
        ApplyCustomValidationsRules();
        ApplyValidationsRules();
    }
    #endregion

    #region Actions
    public void ApplyValidationsRules()
    {
        RuleFor(x => x.NameAr)
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
            .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
            .MaximumLength(50).WithMessage("Max Length is 50");

        RuleFor(x => x.NameEn)
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
            .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
            .MaximumLength(50).WithMessage("Max Length is 50");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
            .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
            .MaximumLength(500).WithMessage("Max Length is 500");

    }
    public void ApplyCustomValidationsRules()
    {
        RuleFor(x => x.NameAr)
        .MustAsync(async (model, Key, CancellationToken) =>
              !await _repositoryManager.CategoryRepository.IsNameExistExceptSelf(x => x.NameAr == model.NameAr && x.Id != model.Id))
        .WithMessage(_localizer[SharedResourcesKeys.IsExist]);

        RuleFor(x => x.NameEn)
           .MustAsync(async (model, Key, CancellationToken) =>
                !await _repositoryManager.CategoryRepository.IsNameExistExceptSelf(x => x.NameEn == model.NameEn && x.Id != model.Id))
           .WithMessage(_localizer[SharedResourcesKeys.IsExist]);
    }

    #endregion
}
