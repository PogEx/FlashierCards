using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Common.Models;

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
        Guid = Guid.Parse(guid);
        Name = name;
        PwHash = pwHash;
        Salt = salt;
    }
    
    public User(){}

    public Guid Guid { get; set; }
    public string Name { get; set; }
    public string PwHash { get; set; }
    public string Salt { get; set; }
}
