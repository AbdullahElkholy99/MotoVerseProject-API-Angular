using MotoVerse.Core.Features.ManagePasswordFeature.Query.Models;

namespace MotoVerse.Core.Features.ManagePasswordFeature.Query.Handles;

public class ManagePasswordQueryHandler : ResponseHandler,
    IRequestHandler<ConfirmResetPasswordQuery, Response<string>>
{


    #region Fields
    private readonly IStringLocalizer<SharedResources> _stringLocalizer;
    private readonly UserManager<User> _userManager;
    private readonly IMediator _mediator;

    #endregion

    #region Constructors
    public ManagePasswordQueryHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                        UserManager<User> userManager,
                                        IMediator mediator) : base(stringLocalizer)
    {
        _stringLocalizer = stringLocalizer;
        _userManager = userManager;
        _mediator = mediator;
    }


    #endregion

    #region Handle Functions

    public async Task<Response<string>> Handle(ConfirmResetPasswordQuery request, CancellationToken cancellationToken)
    {
        var result = await ConfirmResetPassword(request.Code, request.Email);
        switch (result)
        {
            case "UserNotFound": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UserIsNotFound]);
            case "Failed": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.InvaildCode]);
            case "Success": return Success<string>("");
            default: return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.InvaildCode]);
        }
    }

    public async Task<string> ConfirmResetPassword(string Code, string Email)
    {
        //Get User
        //user
        var user = await _userManager.FindByEmailAsync(Email);
        //user not Exist => not found
        if (user == null)
            return "UserNotFound";
        //Decrept Code From Database User Code
        var userCode = user.Code;
        //Equal With Code
        if (userCode == Code) return "Success";
        return "Failed";
    }
    #endregion
}

