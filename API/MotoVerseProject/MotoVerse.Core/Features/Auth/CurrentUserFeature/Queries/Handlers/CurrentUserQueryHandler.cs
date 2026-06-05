using MotoVerse.Core.Features.CurrentUserFeature.Queries.Models;

namespace MotoVerse.Core.Features.CurrentUserFeature.Queries.Handlers;

public class CurrentUserQueryHandler : ResponseHandler,
    IRequestHandler<GetUserQuery, Response<User>>,
    IRequestHandler<GetUserIdQuery, Response<string>>,
    IRequestHandler<GetCurrentUserRolesQuery, Response<List<string>>>
{
    #region Fields

    private readonly IMapper _mapper;
    private readonly IStringLocalizer<SharedResources> _sharedResources;
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    #endregion

    #region Constructors

    public CurrentUserQueryHandler(
        IStringLocalizer<SharedResources> stringLocalizer,
        IMapper mapper,
        UserManager<User> userManager,
        IHttpContextAccessor httpContextAccessor)
        : base(stringLocalizer)
    {
        _mapper = mapper;
        _sharedResources = stringLocalizer;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    #endregion

    #region Handle Functions

    public async Task<Response<User>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await GetUserAsync();
        return Success(user);
    }

    public Task<Response<string>> Handle(GetUserIdQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Success(GetUserId()));
    }

    public async Task<Response<List<string>>> Handle(GetCurrentUserRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await GetCurrentUserRolesAsync();
        return Success(roles);
    }

    #endregion

    #region Helpers

    private string GetUserId()
    {
        return _httpContextAccessor.HttpContext?.User?
                   .Claims
                   .FirstOrDefault(c => c.Type == nameof(UserClaimModel.Id))
                   ?.Value
               ?? throw new UnauthorizedAccessException("User is not authenticated.");
    }

    private async Task<User> GetUserAsync()
    {
        var userId = GetUserId();

        return await _userManager.FindByIdAsync(userId)
               ?? throw new UnauthorizedAccessException("User not found.");
    }

    private async Task<List<string>> GetCurrentUserRolesAsync()
    {
        var user = await GetUserAsync();
        var roles = await _userManager.GetRolesAsync(user);

        return roles.ToList();
    }

    #endregion
}