using Microsoft.AspNetCore.Identity;

namespace ThothDeskCore.Infrastructure;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
}

