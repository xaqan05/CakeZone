using System.ComponentModel.DataAnnotations;

namespace CakeZone.BL.ViewModels.User;

public class LoginVM
{
    [Required]
    public string UsernameOrEmail { get; set; } = null!;

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    public bool RememberMe { get; set; } = false;
}
