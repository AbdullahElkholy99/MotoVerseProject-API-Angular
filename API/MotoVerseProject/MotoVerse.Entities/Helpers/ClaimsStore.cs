using System.Security.Claims;

namespace MotoVerse.Data.Helpers;

public static class ClaimsStore
{
    public static List<Claim> claims = new()
    {
        new Claim(type: "Create Product","false"),
        new Claim(type: "Edit Product","false"),
        new Claim(type: "Delete Product","false"),
    };
}
