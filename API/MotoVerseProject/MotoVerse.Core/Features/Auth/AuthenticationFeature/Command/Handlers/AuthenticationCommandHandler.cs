
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

    #endregion

    #region Constructors
    public AuthenticationCommandHandler(
        IStringLocalizer<SharedResources> stringLocalizer,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IMediator mediator,
        IHttpContextAccessor httpContextAccessor,
        IUrlHelper urlHelper) : base(stringLocalizer)
    {
        _stringLocalizer = stringLocalizer;
        _userManager = userManager;
        _signInManager = signInManager;
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
        _urlHelper = urlHelper;
    }


    #endregion

    #region Handle Functions
    public async Task<Response<JwtAuthResult>> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
            return BadRequest<JwtAuthResult>(
                _stringLocalizer[SharedResourcesKeys.UserNameIsNotExist]);

        var signInResult = await _signInManager.CheckPasswordSignInAsync(
            user,
            request.Password,
            false);

        if (!signInResult.Succeeded)
            return BadRequest<JwtAuthResult>(
                _stringLocalizer[SharedResourcesKeys.PasswordNotCorrect]);

        // Email not confirmed
        if (!user.EmailConfirmed)
        {
            // Generate new confirmation token
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var encryptedEmail = (await _mediator.Send(
                new EncryptionProviderCommand
                {
                    Data = user.Email!
                })).Data;

            var encryptedCode = (await _mediator.Send(
                new EncryptionProviderCommand
                {
                    Data = code
                })).Data;

            var requestAccessor = _httpContextAccessor.HttpContext!.Request;

            var confirmationUrl =
                $"{requestAccessor.Scheme}://{requestAccessor.Host}" +
                $"/api/V1/ConfirmEmail/confirm-email" +
                $"?email={encryptedEmail}&code={encryptedCode}";

            await _mediator.Send(new SendEmailCommand
            {
                Email = user.Email!,
                Subject = "Confirm Your Email",
                Message =
                    $"Please confirm your email by clicking " +
                    $"<a href='{confirmationUrl}'>here</a>"
            });

            return BadRequest<JwtAuthResult>(
                new
                {
                    ConfirmEmail = false,
                    EmailSent = true
                },
                "Email is not confirmed. A new confirmation email has been sent.");
        }

        var result = (await _mediator.Send(
            new GenerateRefreshTokenCommand
            {
                User = user
            })).Data;

        return Success(result);
    }
    public async Task<Response<string>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _signInManager.SignOutAsync();
            return Success("Success Logout");
        }
        catch (Exception)
        {
            return BadRequest<string>("Failed Logout");

            throw;
        }
    }

    #endregion
}
