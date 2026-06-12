using MotoVerse.Core.Features.Auth.ManagePasswordFeature.Command.Models;

namespace MotoVerse.Core.Features.Auth.ManagePasswordFeature.Command.Validators;

public class ChangeUserPasswordValidator : AbstractValidator<ChangeUserPasswordCommand>
{
    #region Fields
    private readonly IStringLocalizer<SharedResources> _localizer;
    #endregion

    #region Constructors
    public ChangeUserPasswordValidator(IStringLocalizer<SharedResources> localizer)
    {
        _localizer = localizer;
        ApplyValidationsRules();
        ApplyCustomValidationsRules();
    }
    #endregion

    #region Handle Functions
    public void ApplyValidationsRules()
    {
        RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);

        RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);

        RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword)
                .WithMessage(_localizer[SharedResourcesKeys.PasswordNotEqualConfirmPass]);
    }

    public void ApplyCustomValidationsRules()
    {

    }

    #endregion
}
