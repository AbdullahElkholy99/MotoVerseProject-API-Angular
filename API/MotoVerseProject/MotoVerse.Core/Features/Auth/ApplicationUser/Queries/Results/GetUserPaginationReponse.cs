namespace MotoVerse.Core.Features.ApplicationUser.Queries.Results;

public class GetUserPaginationReponse
{
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public string Phone { get; set; }
    public string ImagePath { get; set; }
    public string Email { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }
    public int OrderCount { get; set; }
    public int RentalCount { get; set; }
}
