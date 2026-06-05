using Microsoft.AspNetCore.Authentication;
using MotoVerse.Core.Features.ExternalLoginFeature.Commands.Models;

namespace MotoVerse.Core.Features.ExternalLoginFeature.Commands.Handlers;

public class ExternalLoginCommandHandler :
    ResponseHandler,
     IRequestHandler<ConfigureExternalPropertiesCommand, Response<AuthenticationProperties>>
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<SharedResources> _sharedResources;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUrlHelper _urlHelper;
    private readonly IMediator _mediator;
    #endregion

    #region Constructors
    public ExternalLoginCommandHandler(
        IStringLocalizer<SharedResources> stringLocalizer,
                              IMapper mapper,
                              UserManager<User> userManager,
                              IHttpContextAccessor httpContextAccessor,
                              AppDbContext context,
                              IUrlHelper urlHelper,
                              IMediator mediator,
                              SignInManager<User> signInManager) : base(stringLocalizer)
    {
        _mapper = mapper;
        _sharedResources = stringLocalizer;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _context = context;
        _urlHelper = urlHelper;
        _mediator = mediator;
        _signInManager = signInManager;
    }


    #endregion

    #region Handle Functions

    public async Task<Response<AuthenticationProperties>> Handle(ConfigureExternalPropertiesCommand request, CancellationToken cancellationToken)
    {
        var properties = _signInManager
            .ConfigureExternalAuthenticationProperties(
                request.Provider,
                request.RedirectUrl!);

        if (properties == null)
            return BadRequest<AuthenticationProperties>("Failed to configure external authentication.");

        return Success(properties);
    }



    #endregion
}
