namespace Backend.Common.Models.User;

public class User
{
    public User(Guid guid, string name, string pwHash, string salt)
    {
        Guid = guid;
        Name = name;
        PwHash = pwHash;
        Salt = salt;
    }
    
    public User(string guid, string name, string pwHash, string salt)
    {
        Guid = System.Guid.Parse(guid);
        Name = name;
        PwHash = pwHash;
        Salt = salt;
    }
    
    public User(string guid, string name)
    {
        Guid = System.Guid.Parse(guid);
        Name = name;
    }
    
    public Guid Guid { get; set; }
    public string Name { get; set; }
    public string? PwHash { get; set; }
    public string? Salt { get; set; }
}
