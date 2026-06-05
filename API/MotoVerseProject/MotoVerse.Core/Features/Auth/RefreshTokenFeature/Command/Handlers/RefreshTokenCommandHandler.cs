using Microsoft.IdentityModel.Tokens;
using MotoVerse.Entities.Models.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MotoVerse.Core.Features.RefreshTokenFeature.Command.Handlers;

public class RefreshTokenCommandHandler : ResponseHandler,
    IRequestHandler<RefreshTokenCommand, Response<JwtAuthResult>>,
    IRequestHandler<GenerateRefreshTokenCommand, Response<JwtAuthResult>>
{
    #region Fields
    private readonly IStringLocalizer<SharedResources> _stringLocalizer;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IRepositoryManager _repositoryManager;
    private readonly JwtSettings _jwtSettings;

    #endregion

    #region Constructors
    public RefreshTokenCommandHandler(
        IStringLocalizer<SharedResources> stringLocalizer,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IRepositoryManager repositoryManager,
        JwtSettings jwtSettings) : base(stringLocalizer)
    {
        _stringLocalizer = stringLocalizer;
        _userManager = userManager;
        _signInManager = signInManager;
        _repositoryManager = repositoryManager;
        _jwtSettings = jwtSettings;
    }


    #endregion

    #region Handle Functions

    public async Task<Response<JwtAuthResult>> Handle(GenerateRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var (jwtToken, accessToken) = await GenerateJWTToken(request.User);
        var refreshToken = GetRefreshToken(request.User.Email);
        var userRefreshToken = new UserRefreshToken
        {
            AddedTime = DateTime.Now,
            ExpiryDate = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpireDate),
            IsUsed = true,
            IsRevoked = false,
            JwtId = jwtToken.Id,
            RefreshToken = refreshToken.TokenString,
            Token = accessToken,
            UserId = request.User.Id,
            Id = Guid.NewGuid().ToString()
        };
        await _repositoryManager.RefreshTokenRepository.AddAsync(userRefreshToken);

        var response = new JwtAuthResult();
        response.refreshToken = refreshToken;
        response.AccessToken = accessToken;

        return Success(response);
    }
    public async Task<Response<JwtAuthResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var jwtToken = ReadJWTToken(request.AccessToken);
        var userIdAndExpireDate = await ValidateDetails(jwtToken, request.AccessToken, request.RefreshToken);
        switch (userIdAndExpireDate)
        {
            case ("AlgorithmIsWrong", null): return Unauthorized<JwtAuthResult>(_stringLocalizer[SharedResourcesKeys.AlgorithmIsWrong]);
            case ("TokenIsNotExpired", null): return Unauthorized<JwtAuthResult>(_stringLocalizer[SharedResourcesKeys.TokenIsNotExpired]);
            case ("RefreshTokenIsNotFound", null): return Unauthorized<JwtAuthResult>(_stringLocalizer[SharedResourcesKeys.RefreshTokenIsNotFound]);
            case ("RefreshTokenIsExpired", null): return Unauthorized<JwtAuthResult>(_stringLocalizer[SharedResourcesKeys.RefreshTokenIsExpired]);
        }
        var (userId, expiryDate) = userIdAndExpireDate;
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound<JwtAuthResult>();
        }
        var result = await GetRefreshToken(user, jwtToken, expiryDate, request.RefreshToken);
        return Success(result);
    }

    #region Helpers
    #region generate

    private async Task<(JwtSecurityToken, string)> GenerateJWTToken(User user)
    {
        Console.WriteLine("Generate Secret:");
        Console.WriteLine(_jwtSettings.Secret);
        var claims = await GetClaims(user);
        var jwtToken = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            expires: DateTime.Now.AddDays(_jwtSettings.AccessTokenExpireDate),
            signingCredentials: new SigningCredentials(

                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtSettings.Secret
                    )
                ),
            SecurityAlgorithms.HmacSha256));
        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        return (jwtToken, accessToken);
    }
    public async Task<JwtAuthResult> GetRefreshToken(User user, JwtSecurityToken jwtToken, DateTime? expiryDate, string refreshToken)
    {
        var (jwtSecurityToken, newToken) = await GenerateJWTToken(user);
        var response = new JwtAuthResult();
        response.AccessToken = newToken;
        var refreshTokenResult = new RefreshToken();
        refreshTokenResult.Email = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(UserClaimModel.Email)).Value;
        refreshTokenResult.DisplayName = user?.DisplayName ?? "Unknown";
        refreshTokenResult.TokenString = refreshToken;
        refreshTokenResult.ExpireAt = (DateTime)expiryDate;
        response.refreshToken = refreshTokenResult;
        return response;

    }
    private RefreshToken GetRefreshToken(string email)
    {
        var refreshToken = new RefreshToken
        {
            ExpireAt = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpireDate),
            Email = email,
            DisplayName = email,
            TokenString = GenerateRefreshToken()
        };
        return refreshToken;
    }
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        var randomNumberGenerate = RandomNumberGenerator.Create();
        randomNumberGenerate.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    public async Task<List<Claim>> GetClaims(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name,user.DisplayName ?? ""),
            new Claim(ClaimTypes.NameIdentifier,user.Id??""),
            new Claim(ClaimTypes.Email,user.Email),
            new Claim("ImagePath",user.ImagePath ??""),
            new Claim(nameof(UserClaimModel.PhoneNumber), user.PhoneNumber),
            new Claim(nameof(UserClaimModel.Id), user.Id.ToString())
        };
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        var userClaims = await _userManager.GetClaimsAsync(user);
        claims.AddRange(userClaims);
        return claims;
    }
    #endregion
    public JwtSecurityToken ReadJWTToken(string accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            throw new ArgumentNullException(nameof(accessToken));
        }
        var handler = new JwtSecurityTokenHandler();
        var response = handler.ReadJwtToken(accessToken);
        return response;
    }

    public async Task<(string, DateTime?)> ValidateDetails(JwtSecurityToken jwtToken, string accessToken, string refreshToken)
    {
        if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
        {
            return ("AlgorithmIsWrong", null);
        }
        if (jwtToken.ValidTo > DateTime.UtcNow)
        {
            return ("TokenIsNotExpired", null);
        }

        //Get User

        var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(UserClaimModel.Id)).Value;

        var userRefreshToken = await _repositoryManager.RefreshTokenRepository.GetTableNoTracking()
                                         .FirstOrDefaultAsync(x => x.Token == accessToken &&
                                                                 x.RefreshToken == refreshToken &&
                                                                 x.UserId == userId);

        if (userRefreshToken == null)
        {
            return ("RefreshTokenIsNotFound", null);
        }

        if (userRefreshToken.ExpiryDate < DateTime.UtcNow)
        {
            userRefreshToken.IsRevoked = true;
            userRefreshToken.IsUsed = false;
            await _repositoryManager.RefreshTokenRepository.UpdateAsync(userRefreshToken);
            return ("RefreshTokenIsExpired", null);
        }
        var expirydate = userRefreshToken.ExpiryDate;

        return (userId, expirydate);
    }

    #endregion
    #endregion
}
