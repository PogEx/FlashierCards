using System.Data;
using System.Web.Helpers;
using Backend.Common.Models.User;
using Backend.RestApi.Contracts.Auth;
using Dapper;
using Dapper.SimpleSqlBuilder;

namespace Backend.RestApi.Authentication;

public class UserHandler(ITokenManager tokenManager, IDbConnection connection) : IUserHandler
{
    public Guid? CreateUser(string name, string password)
    {
        if (GetUser(name) is not null)
            return null;
        
        string salt = Crypto.GenerateSalt();
        string hashedPassword = Crypto.HashPassword(salt + password);
        
        Guid userGuid = Guid.NewGuid();
        User user = new(){Id = userGuid, Name = name, PasswordHash = hashedPassword, Salt = salt};

        Builder builder = SimpleBuilder.Create($"INSERT INTO user(user_id, name, password_hash, salt) VALUES ({user.Id}, {user.Name}, {user.PasswordHash}, {user.Salt})");
        connection.Execute(builder.Sql, builder.Parameters);
        return userGuid;
    }

    public string? Login(Guid user, string password)
    {
        Builder sql = SimpleBuilder.Create($"SELECT user_id as Id, name as Name, password_hash as PasswordHash, salt as Salt from user WHERE user_id = {user.ToString()}");
        User? result = connection.QueryFirstOrDefault<User>(sql.Sql, sql.Parameters);
        
        if (result is null)
            return null;
        
        if (!Crypto.VerifyHashedPassword(result.PasswordHash, result.Salt + password)) 
            return null;
        
        return tokenManager.GenerateTokenFor(result.Id, result.Name);
    }

    public void Logout(Guid user)
    {
        //TODO
        //invalidate current token for user
    }

    public User? GetUser(string name)
    {
        Builder sql = SimpleBuilder.Create($"SELECT user_id as Id, name from user WHERE name = {name} LIMIT 1");
        return connection.QueryFirstOrDefault<User>(sql.Sql, sql.Parameters);
    }

    public User? GetUser(Guid guid)
    {
        Builder sql = SimpleBuilder.Create($"SELECT user_id as Id, name from user WHERE user_id = {guid} LIMIT 1");
        return connection.QueryFirstOrDefault<User>(sql.Sql, sql.Parameters);
    }
}