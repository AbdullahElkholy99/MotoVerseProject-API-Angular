using MotoVerse.Core.Features.Auth.ConfirmEmailFeature.Command.Models;

namespace MotoVerse.Core.Features.Auth.ConfirmEmailFeature.Command.Handlers;

public class EmailConfirmationHandler : ResponseHandler,
    IRequestHandler<SendEmailConfirmationCommand, Response<bool>>,
    IRequestHandler<SendConfirmEmailQuery, Response<bool>>
{
    #region Fields
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUrlHelper _urlHelper;
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<SharedResources> _stringLocalizer;
    #endregion

    #region Constructors
    public EmailConfirmationHandler(IStringLocalizer<SharedResources> stringLocalizer,
                              IMapper mapper,
                              UserManager<User> userManager,
                              IHttpContextAccessor httpContextAccessor,
                              AppDbContext context,
                              IUrlHelper urlHelper,
                              IMediator mediator) : base(stringLocalizer)
    {
        _stringLocalizer = stringLocalizer;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _urlHelper = urlHelper;
        _mediator = mediator;
    }
    #endregion

    public async Task<Response<bool>> Handle(SendConfirmEmailQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                return BadRequest<bool>(_stringLocalizer[SharedResourcesKeys.ErrorWhenConfirmEmail]);

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return BadRequest<bool>(_stringLocalizer[SharedResourcesKeys.UserIsNotFound]);

            await GenerateEmailConfirmationTokenAsync(user);

            return Success(true, _stringLocalizer[SharedResourcesKeys.SendConfirmEmailDone]);

        }
        catch (Exception)
        {
            return BadRequest<bool>(false, _stringLocalizer[SharedResourcesKeys.SendConfirmEmailFail]);
        }
    }
    public async Task<Response<bool>> Handle(SendEmailConfirmationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await GenerateEmailConfirmationTokenAsync(request.User);

            return Success(true, _stringLocalizer[SharedResourcesKeys.SendConfirmEmailDone]);
        }
        catch (Exception)
        {
            return BadRequest<bool>(false, _stringLocalizer[SharedResourcesKeys.SendConfirmEmailFail]);
        }
    }
    async Task GenerateEmailConfirmationTokenAsync(User user)
    {
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var httpRequest = _httpContextAccessor.HttpContext!.Request;

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

        var urlActionContext = new UrlActionContext
        {
            Action = "ConfirmEmail",
            Controller = "ConfirmEmail",
            Values = new
            {
                Email = encryptedEmail,
                code = encryptedCode
            }
        };

        var returnUrl =
            $"{httpRequest.Scheme}://{httpRequest.Host}" +
            _urlHelper.Action(urlActionContext);

        var message =
            $"To Confirm Email Click Link: <a href='{returnUrl}'>Confirm Email</a>";

        await _mediator.Send(new SendEmailCommand
        {
            Email = user.Email!,
            Subject = "Confirm Your Email",
            Message = message
        });
    }

}