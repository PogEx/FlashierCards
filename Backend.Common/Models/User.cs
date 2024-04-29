namespace Backend.Common.Models;

public record User(Guid Guid, string Name, string PwHash, string Salt);
