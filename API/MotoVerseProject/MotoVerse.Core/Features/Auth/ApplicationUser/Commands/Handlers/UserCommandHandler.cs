using MotoVerse.Core.Features.Auth.ConfirmEmailFeature.Command.Models;
using MotoVerse.Entities.Models.Users;

namespace MotoVerse.Core.Features.ApplicationUser.Commands.Handlers;

public class UserCommandHandler : ResponseHandler,
    IRequestHandler<AddCustomerCommand, Response<string>>,
    IRequestHandler<DeleteUserCommand, Response<string>>,
    IRequestHandler<UpdateUserInfoCommand, Response<string>>,

    IRequestHandler<EditUserCommand, Response<string>>
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
    public async Task<Response<string>> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
    {
        var trans = await _context.Database.BeginTransactionAsync();
        try
        {
            var user = _mapper.Map<Customer>(request);

            //Create
            var createResult = await _userManager.CreateAsync(user, request.Password);

            //Failed
            if (!createResult.Succeeded)
                return BadRequest<string>(_sharedResources[SharedResourcesKeys.FaildToAddUser]);
            var roleResult = await _userManager.AddToRoleAsync(user, "Customer");

            if (!roleResult.Succeeded)
                return BadRequest<string>(_sharedResources[SharedResourcesKeys.FailedToAssignRole]);

            await trans.CommitAsync();

            // send confirmation email  : 
            await _mediator.Send(new SendEmailConfirmationCommand()
            {
                User = user
            });

            return Success<string>("Register is Success", message: "Register is Success");
        }
        catch (Exception ex)
        {
            return BadRequest<string>(_sharedResources[SharedResourcesKeys.TryToRegisterAgain]);
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
    public async Task<Response<string>> Handle(UpdateUserInfoCommand request, CancellationToken cancellationToken)
    {
        var userId = (await _mediator.Send(new GetUserIdQuery())).Data;

        if (userId is null)
            return BadRequest<string>((string)_sharedResources[SharedResourcesKeys.ChangePassFailed]);

        //check if user is exist
        var oldUser = await _userManager.FindByIdAsync(userId);

        //if Not Exist notfound
        if (oldUser == null) return NotFound<string>();

        oldUser.DisplayName = request.Name;
        oldUser.Email = request.Email;
        oldUser.PhoneNumber = request.PhoneNumber;

        //update
        var result = await _userManager.UpdateAsync(oldUser);

        //result is not success
        if (!result.Succeeded) return BadRequest<string>(_sharedResources[SharedResourcesKeys.UpdateFailed]);

        //message
        return Success((string)_sharedResources[SharedResourcesKeys.Updated]);
    }

    #endregion

    #region Delete User
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
        if (!result.Succeeded)
            return BadRequest<string>(_sharedResources[SharedResourcesKeys.DeletedFailed]);

        return Success(message, message: message);
    }

    #endregion


    #endregion
}
