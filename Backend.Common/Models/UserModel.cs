namespace Backend.Common.Models;

public record UserModel(string Name, string PwHash, string Salt);
