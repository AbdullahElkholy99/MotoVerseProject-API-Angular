using MotoVerse.Core.Features.Auth.ConfirmEmailFeature.Command.Models;

namespace MotoVerse.Core.Features.AuthenticationFeature.Command.Handlers;

public class AuthenticationCommandHandler : ResponseHandler,
    IRequestHandler<SignInCommand, Response<JwtAuthResult>>,
    IRequestHandler<LogoutCommand, Response<string>>
{

    #region Fields
    private readonly IStringLocalizer<SharedResources> _stringLocalizer;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUrlHelper _urlHelper;
    private readonly IRepositoryManager _repositoryManager;

    #endregion

    #region Constructors
    public AuthenticationCommandHandler(
        IStringLocalizer<SharedResources> stringLocalizer,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IMediator mediator,
        IHttpContextAccessor httpContextAccessor,
        IUrlHelper urlHelper,
        IRepositoryManager repositoryManager) : base(stringLocalizer)
    {
        _stringLocalizer = stringLocalizer;
        _userManager = userManager;
        _signInManager = signInManager;
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
        _urlHelper = urlHelper;
        _repositoryManager = repositoryManager;
    }


    #endregion

    #region Handle Functions
    public async Task<Response<JwtAuthResult>> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
            return BadRequest<JwtAuthResult>(_stringLocalizer[SharedResourcesKeys.LoginFailed], _stringLocalizer[SharedResourcesKeys.LoginFailed]);

        var checkPassword =
            await _userManager.CheckPasswordAsync(user, request.Password);
        //await _signInManager.CheckPasswordSkignInAsync(user, request.Password, false);

        if (!checkPassword)
            return BadRequest<JwtAuthResult>(_stringLocalizer[SharedResourcesKeys.LoginFailed]);

        // Email not confirmed
        if (!user.EmailConfirmed)
        {
            // Generate new confirmation token
            //BackgroundJob.Enqueue<IMediator>(
            //    svc => svc.Send(new SendEmailConfirmationCommand()
            //    {
            //        User = user
            //    }));
            await _mediator.Send(new SendEmailConfirmationCommand()
            {
                User = user
            });

            return BadRequest<JwtAuthResult>(
                new
                {
                    ConfirmEmail = false,
                    EmailSent = true
                }, "Email is not confirmed. A new confirmation email has been sent.");
        }

        var token = (await _mediator.Send(
            new GenerateAccessTokenCommand
            {
                User = user
            })).Data;

        return Success(token, message: "Success Login");
    }
    public async Task<Response<string>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await _signInManager.SignOutAsync();

        return Success("Logout Successfully");
    }

    #endregion
}
