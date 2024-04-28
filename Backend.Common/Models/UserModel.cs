namespace Backend.Common.Models;

public record UserModel(Guid Guid, string Name, string PwHash, string Salt);
