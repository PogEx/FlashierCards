namespace Backend.Common.Models.Auth;

public record TokenLease(string Token, DateTime ExpiryTime);