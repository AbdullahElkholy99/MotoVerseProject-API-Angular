
//namespace MotoVerse.Service.Implementations;

//public class AuthenticationService : IAuthenticationService
//{
//    #region Fields
//    private readonly JwtSettings _jwtSettings;
//    private readonly IRepositoryManager _repositoryManager;
//    private readonly UserManager<User> _userManager;
//    private readonly AppDbContext _applicationDBContext;
//    #endregion 

//    #region Constructors
//    public AuthenticationService(JwtSettings jwtSettings,
//                                 UserManager<User> userManager,
//                                 AppDbContext applicationDBContext,
//                                 IRepositoryManager repositoryManager)
//    {
//        _jwtSettings = jwtSettings;
//        _userManager = userManager;
//        _applicationDBContext = applicationDBContext;
//        _repositoryManager = repositoryManager;
//    }


//    #endregion

//    #region Handle Functions

//    public async Task<JwtAuthResult> GetJWTToken(User user)
//    {
//        var (jwtToken, accessToken) = await GenerateJWTToken(user);
//        var refreshToken = GetRefreshToken(user.Email);
//        var userRefreshToken = new UserRefreshToken
//        {
//            AddedTime = DateTime.Now,
//            ExpiryDate = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpireDate),
//            IsUsed = true,
//            IsRevoked = false,
//            JwtId = jwtToken.Id,
//            RefreshToken = refreshToken.TokenString,
//            Token = accessToken,
//            UserId = user.Id,
//            Id = Guid.NewGuid().ToString()
//        };
//        await _repositoryManager.RefreshTokenRepository.AddAsync(userRefreshToken);

//        var response = new JwtAuthResult();
//        response.refreshToken = refreshToken;
//        response.AccessToken = accessToken;

//        return response;
//    }

//    private async Task<(JwtSecurityToken, string)> GenerateJWTToken(User user)
//    {
//        var claims = await GetClaims(user);
//        var jwtToken = new JwtSecurityToken(
//            _jwtSettings.Issuer,
//            _jwtSettings.Audience,
//            claims,
//            expires: DateTime.Now.AddDays(_jwtSettings.AccessTokenExpireDate),
//            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)),
//            SecurityAlgorithms.HmacSha256));
//        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
//        return (jwtToken, accessToken);
//    }

//    private RefreshToken GetRefreshToken(string email)
//    {
//        var refreshToken = new RefreshToken
//        {
//            ExpireAt = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpireDate),
//            Email = email,
//            DisplayName = email,
//            TokenString = GenerateRefreshToken()
//        };
//        return refreshToken;
//    }
//    private string GenerateRefreshToken()
//    {
//        var randomNumber = new byte[32];
//        var randomNumberGenerate = RandomNumberGenerator.Create();
//        randomNumberGenerate.GetBytes(randomNumber);
//        return Convert.ToBase64String(randomNumber);
//    }
//    public async Task<List<Claim>> GetClaims(User user)
//    {
//        var roles = await _userManager.GetRolesAsync(user);
//        var claims = new List<Claim>()
//        {
//            new Claim(ClaimTypes.Name,user.UserName),
//            new Claim(ClaimTypes.NameIdentifier,user.UserName),
//            new Claim(ClaimTypes.Email,user.Email),
//            new Claim(nameof(UserClaimModel.PhoneNumber), user.PhoneNumber),
//            new Claim(nameof(UserClaimModel.Id), user.Id.ToString())
//        };
//        foreach (var role in roles)
//        {
//            claims.Add(new Claim(ClaimTypes.Role, role));
//        }
//        var userClaims = await _userManager.GetClaimsAsync(user);
//        claims.AddRange(userClaims);
//        return claims;
//    }

//    public async Task<JwtAuthResult> GetRefreshToken(User user, JwtSecurityToken jwtToken, DateTime? expiryDate, string refreshToken)
//    {
//        var (jwtSecurityToken, newToken) = await GenerateJWTToken(user);
//        var response = new JwtAuthResult();
//        response.AccessToken = newToken;
//        var refreshTokenResult = new RefreshToken();
//        refreshTokenResult.Email = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(UserClaimModel.Email)).Value;
//        refreshTokenResult.DisplayName = user?.DisplayName ?? "Unknown";
//        refreshTokenResult.TokenString = refreshToken;
//        refreshTokenResult.ExpireAt = (DateTime)expiryDate;
//        response.refreshToken = refreshTokenResult;
//        return response;

//    }
//    public JwtSecurityToken ReadJWTToken(string accessToken)
//    {
//        if (string.IsNullOrEmpty(accessToken))
//        {
//            throw new ArgumentNullException(nameof(accessToken));
//        }
//        var handler = new JwtSecurityTokenHandler();
//        var response = handler.ReadJwtToken(accessToken);
//        return response;
//    }

//    public async Task<string> ValidateToken(string accessToken)
//    {
//        var handler = new JwtSecurityTokenHandler();
//        var parameters = new TokenValidationParameters
//        {
//            ValidateIssuer = _jwtSettings.ValidateIssuer,
//            ValidIssuers = new[] { _jwtSettings.Issuer },
//            ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)),
//            ValidAudience = _jwtSettings.Audience,
//            ValidateAudience = _jwtSettings.ValidateAudience,
//            ValidateLifetime = _jwtSettings.ValidateLifeTime,
//        };
//        try
//        {
//            var validator = handler.ValidateToken(accessToken, parameters, out SecurityToken validatedToken);

//            if (validator == null)
//            {
//                return "InvalidToken";
//            }

//            return "NotExpired";
//        }
//        catch (Exception ex)
//        {
//            return ex.Message;
//        }
//    }

//    public async Task<(string, DateTime?)> ValidateDetails(JwtSecurityToken jwtToken, string accessToken, string refreshToken)
//    {
//        if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
//        {
//            return ("AlgorithmIsWrong", null);
//        }
//        if (jwtToken.ValidTo > DateTime.UtcNow)
//        {
//            return ("TokenIsNotExpired", null);
//        }

//        //Get User

//        var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(UserClaimModel.Id)).Value;
//        var userRefreshToken = await _repositoryManager.RefreshTokenRepository.GetTableNoTracking()
//                                         .FirstOrDefaultAsync(x => x.Token == accessToken &&
//                                                                 x.RefreshToken == refreshToken &&
//                                                                 x.UserId == userId);
//        if (userRefreshToken == null)
//        {
//            return ("RefreshTokenIsNotFound", null);
//        }

//        if (userRefreshToken.ExpiryDate < DateTime.UtcNow)
//        {
//            userRefreshToken.IsRevoked = true;
//            userRefreshToken.IsUsed = false;
//            await _repositoryManager.RefreshTokenRepository.UpdateAsync(userRefreshToken);
//            return ("RefreshTokenIsExpired", null);
//        }
//        var expirydate = userRefreshToken.ExpiryDate;
//        return (userId, expirydate);
//    }



//    public async Task<string> SendResetPasswordCode(string Email)
//    {
//        var trans = await _applicationDBContext.Database.BeginTransactionAsync();
//        try
//        {
//            //user
//            var user = await _userManager.FindByEmailAsync(Email);
//            //user not Exist => not found
//            if (user == null)
//                return "UserNotFound";
//            //Generate Random Number

//            //Random generator = new Random();
//            //string randomNumber = generator.Next(0, 1000000).ToString("D6");
//            var chars = "0123456789";
//            var random = new Random();
//            var randomNumber = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());

//            //update User In Database Code
//            user.Code = randomNumber;
//            var updateResult = await _userManager.UpdateAsync(user);
//            if (!updateResult.Succeeded)
//                return "ErrorInUpdateUser";
//            var message = "Code To Reset Passsword : " + user.Code;
//            //Send Code To  Email 
//            await _emailService.SendEmail(user.Email, message, "Reset Password");
//            await trans.CommitAsync();
//            return "Success";
//        }
//        catch (Exception ex)
//        {
//            await trans.RollbackAsync();
//            return "Failed";
//        }
//    }

//    public async Task<string> ConfirmResetPassword(string Code, string Email)
//    {
//        //Get User
//        //user
//        var user = await _userManager.FindByEmailAsync(Email);
//        //user not Exist => not found
//        if (user == null)
//            return "UserNotFound";
//        //Decrept Code From Database User Code
//        var userCode = user.Code;
//        //Equal With Code
//        if (userCode == Code) return "Success";
//        return "Failed";
//    }

//    public async Task<string> ResetPassword(string Email, string Password)
//    {
//        var trans = await _applicationDBContext.Database.BeginTransactionAsync();
//        try
//        {
//            //Get User
//            var user = await _userManager.FindByEmailAsync(Email);
//            //user not Exist => not found
//            if (user == null)
//                return "UserNotFound";
//            await _userManager.RemovePasswordAsync(user);
//            if (!await _userManager.HasPasswordAsync(user))
//            {
//                await _userManager.AddPasswordAsync(user, Password);
//            }
//            await trans.CommitAsync();
//            return "Success";
//        }
//        catch (Exception ex)
//        {
//            await trans.RollbackAsync();
//            return "Failed";
//        }
//    }

//    #endregion
//}
