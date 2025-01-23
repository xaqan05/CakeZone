using CakeZone.CORE.Models.Common;

namespace CakeZone.CORE.Models;

public class Chef : BaseEntity
{
    public string FullName { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public int? DesignationId { get; set; }
    public Designation? Designation { get; set; }
}
