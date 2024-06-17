namespace Backend.Common.Models.Users;

public class UserCreateData
{
    public string Name { get; set; }
    
    public string Password { get; set; }
    
    public string PasswordConfirm { get; set; }
}