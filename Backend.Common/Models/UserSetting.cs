namespace Backend.Common.Models;

public partial class UserSetting
{
    public Guid? UserId { get; set; } = null!;

    public bool IsDark { get; set; }

    public virtual User User { get; set; } = null!;
}
