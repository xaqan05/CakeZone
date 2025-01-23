using System.ComponentModel.DataAnnotations;

namespace CakeZone.BL.ViewModels.User;

public class RegisterVM
{
    [Required]
    public string FullName { get; set; } = null!;

    [Required]
    public string Username { get; set; } = null!;

    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [DataType(DataType.Password), Compare(nameof(Password))]
    public string RePassword { get; set; } = null!;

}
