//using MotoVerse.Services.IService.Base;
//using MotoVerse.Services.Service.Auth;

//namespace MotoVerse.Services.Service.Base;

//public class ServiceManager : IServiceManager
//{
//    #region Auth
//    private readonly Lazy<ICurrentUserService> _currentUserService;
//    private readonly IAuthorizationService _authorizationService;
//    #endregion


//    //private readonly Lazy<IImageService> _ImageService;

//    public ServiceManager(
//        AppDbContext dbContext,
//        IRepositoryManager repositoryManager,
//        IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, IUrlHelper urlHelper,
//         IAuthorizationService authorizationService)
//    {
//        #region Auth
//        _currentUserService = new Lazy<ICurrentUserService>(() => new CurrentUserService(httpContextAccessor, userManager));
//        _authorizationService = authorizationService;
//        #endregion


//        //_ImageService = new Lazy<IImageService>(() => new ImageService());
//    }

//    #region Auth
//    public IAuthorizationService AuthorizationService => _authorizationService;
//    public ICurrentUserService CurrentUserService => _currentUserService.Value;

//    #endregion

//}