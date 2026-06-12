
namespace MotoVerse.Core.Features.RefreshTokenFeature.Command.Handlers;

public class RefreshTokenCommandHandler : ResponseHandler,
    //IRequestHandler<GenerateRefreshTokenCommand, Response<JwtAuthResult>>,
    IRequestHandler<GenerateAccessTokenCommand, Response<JwtAuthResult>>
{
    #region Fields
    private readonly IStringLocalizer<SharedResources> _stringLocalizer;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IRepositoryManager _repositoryManager;
    private readonly JwtSettings _jwtSettings;
    private readonly IConfiguration _configuration;

    #endregion

    #region Constructors
    public RefreshTokenCommandHandler(
        IStringLocalizer<SharedResources> stringLocalizer,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IRepositoryManager repositoryManager,
        JwtSettings jwtSettings,
        IConfiguration configuration) : base(stringLocalizer)
    {
        _stringLocalizer = stringLocalizer;
        _userManager = userManager;
        _signInManager = signInManager;
        _repositoryManager = repositoryManager;
        _jwtSettings = jwtSettings;
        _configuration = configuration;
    }


    #endregion

    #region Handle Functions

    //public async Task<Response<JwtAuthResult>> Handle(GenerateRefreshTokenCommand request, CancellationToken cancellationToken)
    //{
    //    var jwtToken = ReadJWTToken(request.AccessToken);
    //    var userIdAndExpireDate = await ValidateDetails(jwtToken, request.AccessToken, request.RefreshToken);
    //    switch (userIdAndExpireDate)
    //    {
    //        case ("AlgorithmIsWrong", null): return Unauthorized<JwtAuthResult>(_stringLocalizer[SharedResourcesKeys.AlgorithmIsWrong]);
    //        case ("TokenIsNotExpired", null): return Unauthorized<JwtAuthResult>(_stringLocalizer[SharedResourcesKeys.TokenIsNotExpired]);
    //        case ("RefreshTokenIsNotFound", null): return Unauthorized<JwtAuthResult>(_stringLocalizer[SharedResourcesKeys.RefreshTokenIsNotFound]);
    //        case ("RefreshTokenIsExpired", null): return Unauthorized<JwtAuthResult>(_stringLocalizer[SharedResourcesKeys.RefreshTokenIsExpired]);
    //    }
    //    var (userId, expiryDate) = userIdAndExpireDate;
    //    var user = await _userManager.FindByIdAsync(userId);
    //    if (user == null)
    //    {
    //        return NotFound<JwtAuthResult>();
    //    }
    //    var result = await GetRefreshToken(user, jwtToken, expiryDate, request.RefreshToken);
    //    return Success(result);
    //}


    public async Task<Response<JwtAuthResult>> Handle(GenerateAccessTokenCommand request, CancellationToken cancellationToken)
    {
        var signingCredentials = GetSigningCredentials();

        var claims = await GetClaims(request.User);

        var (jwtToken, accessToken) = await GenerateJWTToken(signingCredentials, claims);
        //JwtAuthResult? refreshToken = await GetRefreshToken(claims, signingCredentials, accessToken);

        var response = new JwtAuthResult();
        response.RefreshToken = null;
        response.AccessToken = accessToken;

        return Success(response);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var secret = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.Secret));

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaims(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.DisplayName??"") ,
            new Claim(ClaimTypes.Email, user.Email??""),
            new Claim("ImagePath",user.ImagePath ??""),
            new Claim(ClaimTypes.NameIdentifier,user.Id??"")
        };
        var roles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        return claims;
    }
    private async Task<(JwtSecurityToken, string)> GenerateJWTToken(SigningCredentials signingCredentials,
        List<Claim> claims)
    {
        var jwtToken = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            expires: DateTime.Now.AddDays(_jwtSettings.AccessTokenExpireDate),
            signingCredentials: signingCredentials
            );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        return (jwtToken, accessToken);
    }
    public async Task<JwtAuthResult> GetRefreshToken(List<Claim> claims, SigningCredentials signingCredentials, string refreshToken)
    {

        var (jwtSecurityToken, newToken) = await GenerateJWTToken(signingCredentials, claims);

        var response = new JwtAuthResult();
        response.AccessToken = newToken;

        var refreshTokenResult = new RefreshToken();
        refreshTokenResult.Email = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        refreshTokenResult.TokenString = refreshToken;
        refreshTokenResult.ExpireAt = DateTime.Now.AddDays(_jwtSettings.AccessTokenExpireDate);
        response.RefreshToken = refreshTokenResult;
        return response;

    }
    #endregion

    #region Helpers


    //private RefreshToken GetRefreshToken(string email)
    //{
    //    var refreshToken = new RefreshToken
    //    {
    //        ExpireAt = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpireDate),
    //        Email = email,
    //        DisplayName = email,
    //        TokenString = GenerateRefreshToken()
    //    };
    //    return refreshToken;
    //}
    //private string GenerateRefreshToken()
    //{
    //    var randomNumber = new byte[32];
    //    var randomNumberGenerate = RandomNumberGenerator.Create();
    //    randomNumberGenerate.GetBytes(randomNumber);
    //    return Convert.ToBase64String(randomNumber);
    //}
    //public async Task<List<Claim>> GetClaims(User user)
    //{
    //    var roles = await _userManager.GetRolesAsync(user);
    //    var claims = new List<Claim>()
    //    {
    //        new Claim(ClaimTypes.Name,user.DisplayName ?? ""),
    //        new Claim(ClaimTypes.NameIdentifier,user.Id??""),
    //        new Claim(ClaimTypes.Email,user.Email),
    //        new Claim("ImagePath",user.ImagePath ??""),
    //        new Claim(nameof(UserClaimModel.PhoneNumber), user.PhoneNumber),
    //        new Claim(nameof(UserClaimModel.Id), user.Id.ToString())
    //    };
    //    foreach (var role in roles)
    //    {
    //        claims.Add(new Claim(ClaimTypes.Role, role));
    //    }
    //    var userClaims = await _userManager.GetClaimsAsync(user);
    //    claims.AddRange(userClaims);
    //    return claims;
    //}
    //#endregion

    //public JwtSecurityToken ReadJWTToken(string accessToken)
    //{
    //    if (string.IsNullOrEmpty(accessToken))
    //    {
    //        throw new ArgumentNullException(nameof(accessToken));
    //    }
    //    var handler = new JwtSecurityTokenHandler();
    //    var response = handler.ReadJwtToken(accessToken);
    //    return response;
    //}

    //public async Task<(string, DateTime?)> ValidateDetails(JwtSecurityToken jwtToken, string accessToken, string refreshToken)
    //{
    //    if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
    //    {
    //        return ("AlgorithmIsWrong", null);
    //    }
    //    if (jwtToken.ValidTo > DateTime.UtcNow)
    //    {
    //        return ("TokenIsNotExpired", null);
    //    }

    //    //Get User

    //    var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(UserClaimModel.Id)).Value;

    //    var userRefreshToken = await _repositoryManager.RefreshTokenRepository.GetTableNoTracking()
    //                                     .FirstOrDefaultAsync(x => x.Token == accessToken &&
    //                                                             x.RefreshToken == refreshToken &&
    //                                                             x.UserId == userId);

    //    if (userRefreshToken == null)
    //    {
    //        return ("RefreshTokenIsNotFound", null);
    //    }

    //    if (userRefreshToken.ExpiryDate < DateTime.UtcNow)
    //    {
    //        userRefreshToken.IsRevoked = true;
    //        userRefreshToken.IsUsed = false;
    //        await _repositoryManager.RefreshTokenRepository.UpdateAsync(userRefreshToken);
    //        return ("RefreshTokenIsExpired", null);
    //    }
    //    var expirydate = userRefreshToken.ExpiryDate;

    //    return (userId, expirydate);
    //}


    #endregion
}
