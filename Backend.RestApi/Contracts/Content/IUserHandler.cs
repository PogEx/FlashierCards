using Backend.Common.Models.Users;
using FluentResults;

namespace Backend.RestApi.Contracts.Content;

public interface IUserHandler
{
    Task<Result<Guid>> CreateUser(string name, string password);
    Task<Result<string>> Login(Guid user, string password);
    Task<Result> Logout(Guid user);
    Task<Result<UserDto>> GetUser(string name);
    Task<Result<UserDto>> GetUser(Guid guid);
}