using Microsoft.AspNetCore.Identity;
using MotoVerse.Entities.Models.Auth;

namespace MotoVerse.Data.Entities.Auth;

public class User : IdentityUser
{
    public string DisplayName { get; set; }
    public string? Address { get; set; }
    public string? ImagePath { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    public string? Code { get; set; }
    [InverseProperty(nameof(UserRefreshToken.user))]
    public virtual ICollection<UserRefreshToken> UserRefreshTokens { get; set; }


}
