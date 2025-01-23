using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CakeZone.BL.ViewModels.Chef;

public class UpdateChefVM
{
    [Required, MaxLength(64)]
    public string FullName { get; set; } = null!;

    public int? DesignationId { get; set; }

    public IFormFile? Image { get; set; }

    public string? ExistImageUrl { get; set; }
}
