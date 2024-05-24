using Microsoft.EntityFrameworkCore;

namespace Backend.Common.Models.DatabaseModels;

[PrimaryKey("Id")]
public class DbCardType
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<DbCard> Cards { get; set; } = new List<DbCard>();
}
