﻿using Microsoft.EntityFrameworkCore;

namespace Backend.Common.Models;

[PrimaryKey("UserId")]
public class User
{
    public Guid UserId { get; set; }

    public string Name { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public virtual ICollection<Folder> Folders { get; set; } = new List<Folder>();

    public virtual UserSetting? UserSetting { get; set; }

    public virtual ICollection<Deck> Decks { get; set; } = new List<Deck>();
}
