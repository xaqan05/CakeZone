using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CakeZone.BL.ViewModels.Chef;

public class CreateChefVM
{
    [Required, MaxLength(64)]
    public string FullName { get; set; } = null!;

    public int? DesignationId { get; set; }

    [Required]
    public IFormFile Image { get; set; } = null!;
}
