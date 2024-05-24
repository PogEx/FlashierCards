using Backend.Common.Models.DatabaseModels;

namespace Backend.Common.Models.User;

public struct User
{
    public User()
    {
    }
    
    public User(DbUser user)
    {
        UserId = user.UserId;
        Name = user.Name;
    }

    public Guid UserId { get; set; } = default;

    public string Name { get; set; } = null!;
}