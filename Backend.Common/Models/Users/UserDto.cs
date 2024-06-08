namespace Backend.Common.Models.Users;

public struct UserDto
{
    public UserDto()
    {
    }

    public Guid UserId { get; set; } = default;

    public string Name { get; set; } = null!;
}