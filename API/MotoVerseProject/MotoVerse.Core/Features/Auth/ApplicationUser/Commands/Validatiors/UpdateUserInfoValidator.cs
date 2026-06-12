namespace MotoVerse.Core.Features.ApplicationUser.Commands.Validatiors;

public class UpdateUserInfoValidator : AbstractValidator<UpdateUserInfoCommand>
{
    #region Fields
    private readonly IStringLocalizer<SharedResources> _localizer;
    private readonly UserManager<User> _userManager;
    private readonly IMediator _mediator;
    #endregion

    #region Constructors
    public UpdateUserInfoValidator(IStringLocalizer<SharedResources> localizer, UserManager<User> userManager, IMediator mediator)
    {
        _localizer = localizer;
        _mediator = mediator;
        _userManager = userManager;
        ApplyValidationsRules();
        ApplyCustomValidationsRules();
    }
    #endregion

    #region Handle Functions
    public void ApplyValidationsRules()
    {
        RuleFor(x => x.Name)
             .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
             .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
             .MaximumLength(12).WithMessage(_localizer[SharedResourcesKeys.MaxLengthis100]);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
            .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);

        RuleFor(x => x.Email)
             .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
             .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);
    }

    public async Task ApplyCustomValidationsRules()
    {
        var userId = (await _mediator.Send(new GetUserIdQuery())).Data;

        RuleFor(x => x.Email)
          .MustAsync(async (email, cancellationToken) =>
          {
              var user = await _userManager.FindByEmailAsync(email);

              return user == null || user.Id == userId;
          })
          .WithMessage(_localizer[SharedResourcesKeys.EmailIsExist]);

    }

    #endregion
}