using CakeZone.CORE.Models.Common;

namespace CakeZone.CORE.Models;

public class Designation : BaseEntity
{
    public string Name { get; set; } = null!;
    public IEnumerable<Chef> Chefs { get; set; } = null!;
}
