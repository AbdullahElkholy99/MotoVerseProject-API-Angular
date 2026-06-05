using MotoVerse.Core.Features.ConfirmEmailFeature.Query.Models;
using MotoVerse.Core.Features.Shared.EmailFeature.Commands.Models;

namespace MotoVerse.Core.Features.ConfirmEmailFeature.Query.Handles;

public class ConfirmEmailQueryHandler : ResponseHandler,
    IRequestHandler<ConfirmEmailQuery, Response<string>>,
    IRequestHandler<SendConfirmEmailQuery, Response<bool>>
{


    #region Fields
    private readonly IStringLocalizer<SharedResources> _stringLocalizer;
    private readonly UserManager<User> _userManager;
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUrlHelper _urlHelper;

    #endregion

    #region Constructors
    public ConfirmEmailQueryHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                        UserManager<User> userManager,
                                        IMediator mediator,
                                        IHttpContextAccessor httpContextAccessor,
                                        IUrlHelper urlHelper) : base(stringLocalizer)
    {
        _stringLocalizer = stringLocalizer;
        _userManager = userManager;
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
        _urlHelper = urlHelper;
    }


    #endregion

    #region Handle Functions

    #region Confirm Email 
    public async Task<Response<string>> Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var email = (await _mediator.Send(
                new DecryptionProviderCommand
                {
                    Data = request.Email
                })).Data;

            var code = (await _mediator.Send(
                new DecryptionProviderCommand
                {
                    Data = request.Code
                })).Data;

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(code))
            {
                return BadRequest<string>(
                    _stringLocalizer[SharedResourcesKeys.ErrorWhenConfirmEmail]);
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return BadRequest<string>(
                    _stringLocalizer[SharedResourcesKeys.UserIsNotFound]);
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    " | ",
                    result.Errors.Select(e => e.Description));

                return BadRequest<string>(errors);
            }

            return Success<string>(
                _stringLocalizer[SharedResourcesKeys.ConfirmEmailDone]);
        }
        catch (Exception ex)
        {
            return BadRequest<string>(ex.Message);
        }
    }
    public async Task<Response<bool>> Handle(SendConfirmEmailQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            return BadRequest<bool>(
                _stringLocalizer[SharedResourcesKeys.ErrorWhenConfirmEmail]);

        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
            return BadRequest<bool>(
                _stringLocalizer[SharedResourcesKeys.UserIsNotFound]);

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

        return Success(
            true,
            _stringLocalizer[SharedResourcesKeys.SendConfirmEmailDone]);
    }
    #endregion


    #endregion
}

