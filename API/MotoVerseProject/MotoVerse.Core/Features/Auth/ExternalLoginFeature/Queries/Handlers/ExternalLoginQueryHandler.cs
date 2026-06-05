using MotoVerse.Core.Features.ExternalLoginFeature.Queries.Models;
using MotoVerse.Core.Features.ExternalLoginFeature.Queries.Results;
using MotoVerse.Entities.Models.Users;
using System.Security.Claims;

namespace LegalFlow.Core.Features.ExternalLoginFeature.Queries.Handlers;

public class ExternalLoginQueryHandler : ResponseHandler,
    IRequestHandler<LoginCommand, Response<AuthExternalResponse>>
{
    #region Fields

    private readonly IMapper _mapper;
    private readonly IStringLocalizer<SharedResources> _sharedResources;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    #endregion

    #region Constructor

    public ExternalLoginQueryHandler(
        IStringLocalizer<SharedResources> stringLocalizer,
        IMapper mapper,
        UserManager<User> userManager,
        SignInManager<User> signInManager)
        : base(stringLocalizer)
    {
        _mapper = mapper;
        _sharedResources = stringLocalizer;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    #endregion

    #region Handle Functions

    public async Task<Response<AuthExternalResponse>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();

        if (info == null)
        {
            return BadRequest<AuthExternalResponse>(
                "Error loading external login information.");
        }

        string loginProvider = info.LoginProvider;
        string providerKey = info.ProviderKey;

        var externalSignInResult =
            await _signInManager.ExternalLoginSignInAsync(
                loginProvider,
                providerKey,
                isPersistent: true);

        User? user = null;

        if (externalSignInResult.Succeeded)
        {
            user = await _userManager
                .FindByLoginAsync(loginProvider, providerKey);
        }
        else
        {
            var email =
                info.Principal?.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrWhiteSpace(email))
            {
                email = $"{providerKey}@{loginProvider}.com";
            }

            user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                var name =
                    info.Principal?.FindFirstValue(ClaimTypes.Name)
                    ?? email;

                string? pictureUrl = null;

                if (loginProvider == "Google")
                {
                    pictureUrl =
                        info.Principal?.FindFirstValue("picture");
                }
                else if (loginProvider == "Facebook")
                {
                    pictureUrl =
                        info.Principal?.FindFirstValue("picture")
                        ?? info.Principal?.FindFirstValue("urn:facebook:picture");
                }

                user = new Customer
                {
                    Email = email,
                    UserName = email,
                    DisplayName = name,
                    EmailConfirmed = true,
                    ImagePath = pictureUrl
                };

                var createResult =
                    await _userManager.CreateAsync(user);

                if (!createResult.Succeeded)
                {
                    return BadRequest<AuthExternalResponse>(
                        "Error creating user.");
                }

                await _userManager.AddToRoleAsync(user, "Trainee");
            }

            var addLoginResult =
                await _userManager.AddLoginAsync(user, info);

            if (!addLoginResult.Succeeded)
            {
                return BadRequest<AuthExternalResponse>(
                    "Login failed.");
            }

            await _signInManager.SignInAsync(
                user,
                isPersistent: true);
        }

        if (user == null)
        {
            return BadRequest<AuthExternalResponse>(
                "Login failed.");
        }

        var roles = await _userManager.GetRolesAsync(user);

        var response = new AuthExternalResponse
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            ImagePath = user.ImagePath,
            Roles = roles.ToList(),
            Message = "Success Login"
        };

        return Success(response);
    }

    #endregion
}