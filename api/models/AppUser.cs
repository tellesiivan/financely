using Microsoft.AspNetCore.Identity;

namespace api.models;

public class AppUser: IdentityUser
{
    public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
}