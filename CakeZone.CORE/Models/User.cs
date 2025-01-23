using Microsoft.AspNetCore.Identity;

namespace CakeZone.CORE.Models;

public class User : IdentityUser
{
    public string FullName { get; set; } = null!;
}
