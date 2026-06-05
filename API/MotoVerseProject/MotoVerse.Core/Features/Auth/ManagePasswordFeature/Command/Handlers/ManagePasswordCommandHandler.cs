using MotoVerse.Core.Features.ManagePasswordFeature.Command.Models;
using MotoVerse.Core.Features.Shared.EmailFeature.Commands.Models;

namespace MotoVerse.Core.Features.ManagePasswordFeature.Command.Handlers;

public class ManagePasswordCommandHandler : ResponseHandler,
    IRequestHandler<SendResetPasswordCommand, Response<string>>,
    IRequestHandler<ResetPasswordCommand, Response<string>>
{


    #region Fields
    private readonly IStringLocalizer<SharedResources> _stringLocalizer;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMediator _mediator;
    private readonly AppDbContext _dbContext;


    #endregion

    #region Constructors
    public ManagePasswordCommandHandler(
        IStringLocalizer<SharedResources> stringLocalizer,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IMediator mediator,
        IRepositoryManager repositoryManager,
        AppDbContext dbContext) : base(stringLocalizer)
    {
        _stringLocalizer = stringLocalizer;
        _userManager = userManager;
        _signInManager = signInManager;
        _mediator = mediator;
        _repositoryManager = repositoryManager;
        _dbContext = dbContext;
    }


    #endregion

    #region Handle Functions

    public async Task<Response<string>> Handle(SendResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var result = await SendResetPasswordCode(request.Email);
        switch (result)
        {
            case "UserNotFound": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UserIsNotFound]);
            case "ErrorInUpdateUser": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.TryAgainInAnotherTime]);
            case "Failed": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.TryAgainInAnotherTime]);
            case "Success": return Success<string>("");
            default: return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.TryAgainInAnotherTime]);
        }
    }

    public async Task<Response<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var result = await ResetPassword(request.Email, request.Password);
        switch (result)
        {
            case "UserNotFound": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.UserIsNotFound]);
            case "Failed": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.InvaildCode]);
            case "Success": return Success<string>("", message: "Send Code Success");
            default: return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.InvaildCode]);
        }
    }

    #region Helper Methods


    public async Task<string> SendResetPasswordCode(string Email)
    {
        var trans = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            //user
            var user = await _userManager.FindByEmailAsync(Email);
            //user not Exist => not found
            if (user == null)
                return "UserNotFound";

            //Generate Random Number
            var chars = "01012613453";
            var random = new Random();
            var randomNumber = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());

            //update User In Database Code
            user.Code = randomNumber;
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded) return "ErrorInUpdateUser";

            var message = "Code To Reset Passsword : " + user.Code;
            //Send Code To  Email 
            await _mediator.Send(new SendEmailCommand()
            {
                Email = user.Email,
                Message = message,
                Subject = "Reset Password"
            });

            await trans.CommitAsync();

            return "Success";
        }
        catch (Exception ex)
        {
            await trans.RollbackAsync();
            return "Failed";
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

    public async Task<string> ResetPassword(string Email, string Password)
    {
        var trans = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            //Get User
            var user = await _userManager.FindByEmailAsync(Email);
            //user not Exist => not found
            if (user == null)
                return "UserNotFound";
            await _userManager.RemovePasswordAsync(user);
            if (!await _userManager.HasPasswordAsync(user))
            {
                await _userManager.AddPasswordAsync(user, Password);
            }
            await trans.CommitAsync();
            return "Success";
        }
        catch (Exception ex)
        {
            await trans.RollbackAsync();
            return "Failed";
        }
    }
    #endregion

    #endregion
}
