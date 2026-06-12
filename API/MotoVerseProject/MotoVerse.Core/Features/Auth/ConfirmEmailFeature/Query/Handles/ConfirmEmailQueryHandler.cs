using MotoVerse.Core.Features.ConfirmEmailFeature.Query.Models;

namespace MotoVerse.Core.Features.ConfirmEmailFeature.Query.Handles;

public class ConfirmEmailQueryHandler : ResponseHandler,
    IRequestHandler<ConfirmEmailQuery, Response<string>>
{


    #region Fields
    private readonly IStringLocalizer<SharedResources> _stringLocalizer;
    private readonly UserManager<User> _userManager;
    private readonly IMediator _mediator;

    #endregion

    #region Constructors
    public ConfirmEmailQueryHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                        UserManager<User> userManager,
                                        IMediator mediator) : base(stringLocalizer)
    {
        _stringLocalizer = stringLocalizer;
        _userManager = userManager;
        _mediator = mediator;
    }


    #endregion

    #region Handle Functions

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

    #endregion
}

