using System.ComponentModel.DataAnnotations;

namespace CakeZone.BL.ViewModels.Designation;

public class UpdateDesignationVM
{
    [MaxLength(64), Required]
    public string Name { get; set; } = null!;
}
