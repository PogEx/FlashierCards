﻿using Microsoft.EntityFrameworkCore;

namespace Backend.Common.Models;

[PrimaryKey("UserId")]
public class UserSetting
{
    public Guid UserId { get; set; }

    public bool IsDark { get; set; }

    public virtual User User { get; set; } = null!;
}
