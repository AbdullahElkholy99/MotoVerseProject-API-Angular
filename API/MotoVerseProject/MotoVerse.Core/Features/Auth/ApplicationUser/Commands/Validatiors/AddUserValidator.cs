namespace MotoVerse.Core.Features.Auth.AuthenticationFeature.Command.Validators;

public class AddUserValidator : AbstractValidator<AddCustomerCommand>
{
    #region Fields
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly UserManager<User> _userManager;
    #endregion

    #region Constructors
    public AddUserValidator(IStringLocalizer<SharedResources> localizer, UserManager<User> userManager)
    {
        _localizer = localizer;
        _userManager = userManager;
        ApplyValidationsRules();
        ApplyCustomValidationsRules();
    }
    #endregion

    #region Handle Functions
    public void ApplyValidationsRules()
    {
        RuleFor(x => x.DisplayName)
             .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
             .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
             .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthis100]);

        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
            .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
            .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthis100]);

        RuleFor(x => x.Email)
             .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
             .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);
        RuleFor(x => x.Password)
             .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
             .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);
        RuleFor(x => x.ConfirmPassword)
             .Equal(x => x.Password).WithMessage(_localizer[SharedResourcesKeys.PasswordNotEqualConfirmPass]);

    }

    public void ApplyCustomValidationsRules()
    {
        RuleFor(x => x.Email)
            .MustAsync(async (email, cancellationToken) =>
            {
                var user = await _userManager.FindByEmailAsync(email);
                return user == null;
            })
            .WithMessage(_localizer[SharedResourcesKeys.EmailIsExist]);


    }

    #endregion
}
