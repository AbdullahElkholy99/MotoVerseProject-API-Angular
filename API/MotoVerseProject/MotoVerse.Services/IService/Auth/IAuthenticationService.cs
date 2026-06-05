//using MotoVerse.Data.Results;
//using System.IdentityModel.Tokens.Jwt;

//namespace MotoVerse.Service.Abstracts;

//public interface IAuthenticationService
//{
//    public Task<JwtAuthResult> GetJWTToken(User user);
//    public JwtSecurityToken ReadJWTToken(string accessToken);
//    public Task<(string, DateTime?)> ValidateDetails(JwtSecurityToken jwtToken, string AccessToken, string RefreshToken);
//    public Task<JwtAuthResult> GetRefreshToken(User user, JwtSecurityToken jwtToken, DateTime? expiryDate, string refreshToken);
//    public Task<string> ValidateToken(string AccessToken);
//    public Task<string> SendResetPasswordCode(string Email);
//    public Task<string> ConfirmResetPassword(string Code, string Email);
//    public Task<string> ResetPassword(string Email, string Password);
//}
