namespace MotoVerse.Core.Features.ApplicationUser.Queries.Results;

public class GetUserByIdResponse
{
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? ImagePath { get; set; }
    public bool IsActive { get; set; }
    public int OrderCount { get; set; }
    public int RentalCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
public class GetUserInfoResponse
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}