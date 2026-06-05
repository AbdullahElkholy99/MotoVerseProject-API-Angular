using MotoVerse.Entities.Models.Users;

namespace MotoVerse.Core.Features.ApplicationUser.Commands.Handlers;

public class UserCommandHandler : ResponseHandler,
    IRequestHandler<AddUserCommand, Response<string>>,
    IRequestHandler<EditUserCommand, Response<string>>,
    IRequestHandler<DeleteUserCommand, Response<string>>,
    IRequestHandler<ChangeUserPasswordCommand, Response<string>>
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<SharedResources> _sharedResources;
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUrlHelper _urlHelper;
    private readonly IMediator _mediator;
    #endregion

    #region Constructors
    public UserCommandHandler(IStringLocalizer<SharedResources> stringLocalizer,
                              IMapper mapper,
                              UserManager<User> userManager,
                              IHttpContextAccessor httpContextAccessor,
                              AppDbContext context,
                              IUrlHelper urlHelper,
                              IMediator mediator) : base(stringLocalizer)
    {
        _mapper = mapper;
        _sharedResources = stringLocalizer;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _context = context;
        _urlHelper = urlHelper;
        _mediator = mediator;
    }


    #endregion

    #region Handle Functions

    #region Add User
    public async Task<Response<string>> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        //var identityUser = request.MapToUser();
        var identityUser = _mapper.Map<Customer>(request);
        //Create
        var createResult = await AddUserAsync(identityUser, request.Password);
        switch (createResult)
        {
            case "EmailIsExist": return BadRequest<string>(_sharedResources[SharedResourcesKeys.EmailIsExist]);
            case "UserNameIsExist": return BadRequest<string>(_sharedResources[SharedResourcesKeys.UserNameIsExist]);
            case "ErrorInCreateUser": return BadRequest<string>(_sharedResources[SharedResourcesKeys.FaildToAddUser]);
            case "Failed": return BadRequest<string>(_sharedResources[SharedResourcesKeys.TryToRegisterAgain]);
            case "Success": return Success<string>("Register is Success");
            default: return BadRequest<string>(createResult);
        }
    }
    public async Task<string> AddUserAsync(User user, string password)
    {
        var trans = await _context.Database.BeginTransactionAsync();
        try
        {
            //if Email is Exist
            var existUser = await _userManager.FindByEmailAsync(user.Email);
            //email is Exist
            if (existUser != null) return "EmailIsExist";


            //Create
            var createResult = await _userManager.CreateAsync(user, password);
            //Failed
            if (!createResult.Succeeded)
                return string.Join(",", createResult.Errors.Select(x => x.Description).ToList());


            await _userManager.AddToRoleAsync(user, "User");

            //Send Confirm Email
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var resquestAccessor = _httpContextAccessor.HttpContext.Request;

            var urlActionContext = new UrlActionContext
            {
                Action = "ConfirmEmail",
                Controller = "ConfirmEmail",
                Values = new
                {
                    userId = (await _mediator.Send(new EncryptionProviderCommand { Data = user.Id.ToString() })).Data,
                    code = (await _mediator.Send(new EncryptionProviderCommand { Data = code })).Data
                }
            };
            var returnUrl = resquestAccessor.Scheme + "://" + resquestAccessor.Host + _urlHelper.Action(urlActionContext);

            var message = $"To Confirm Email Click Link: <a href='{returnUrl}'>Link Of Confirmation</a>";

            //message or body
            await _mediator.Send(new SendEmailCommand
            {
                Email = user.Email,
                Message = message,
                Subject = "Confirm Your Email"
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


    #endregion

    #region Edit User
    public async Task<Response<string>> Handle(EditUserCommand request, CancellationToken cancellationToken)
    {
        //check if user is exist
        var oldUser = await _userManager.FindByIdAsync(request.Id.ToString());
        //if Not Exist notfound
        if (oldUser == null) return NotFound<string>();
        //mapping
        var newUser = _mapper.Map(request, oldUser);

        //if username is Exist
        var userByUserName = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == newUser.UserName && x.Id != newUser.Id);
        //username is Exist
        if (userByUserName != null) return BadRequest<string>(_sharedResources[SharedResourcesKeys.UserNameIsExist]);

        //update
        var result = await _userManager.UpdateAsync(newUser);
        //result is not success
        if (!result.Succeeded) return BadRequest<string>(_sharedResources[SharedResourcesKeys.UpdateFailed]);
        //message
        return Success((string)_sharedResources[SharedResourcesKeys.Updated]);
    }

    #endregion

    #region Delete User
    //  15e89450-05c4-4fbb-b3c0-5f349ae9afbd
    public async Task<Response<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        //check if user is exist
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        //if Not Exist notfound
        if (user == null) return NotFound<string>();
        string message = user.IsActive ? "Success Deleted" : "Success Activated";
        //Delete the User
        user.IsActive = !user.IsActive;
        var result = await _userManager.UpdateAsync(user);
        //in case of Failure
        if (!result.Succeeded) return BadRequest<string>(_sharedResources[SharedResourcesKeys.DeletedFailed]);

        return Success(message, message: message);
    }

    #endregion


    #region change password
    public async Task<Response<string>> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
    {
        //get user
        //check if user is exist
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        //if Not Exist notfound
        if (user == null) return NotFound<string>();

        //Change User Password
        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        //var user1=await _userManager.HasPasswordAsync(user);
        //await _userManager.RemovePasswordAsync(user);
        //await _userManager.AddPasswordAsync(user, request.NewPassword);

        //result
        if (!result.Succeeded) return BadRequest<string>(result.Errors.FirstOrDefault().Description);
        return Success((string)_sharedResources[SharedResourcesKeys.Success]);
    }

    #endregion

    #endregion
}
