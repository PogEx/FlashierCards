using Microsoft.EntityFrameworkCore;

namespace Backend.Common.Models.DatabaseModels;

[PrimaryKey("UserId")]
public class DbUserSetting
{
    public Guid UserId { get; set; }

    public bool IsDark { get; set; }

    public virtual DbUser DbUser { get; set; } = null!;
}
