using System.Data;
using System.Web.Helpers;
using Backend.Common.Models;
using Backend.Common.Models.Auth;
using Backend.RestApi.Contracts.Auth;
using Dapper;
using Dapper.SimpleSqlBuilder;
using Dapper.SimpleSqlBuilder.FluentBuilder;

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
        User user = new(userGuid.ToString(), name, hashedPassword, salt);

        IFluentSqlBuilder builder = SimpleBuilder.CreateFluent()
            .InsertInto($"user")
            .Columns($"guid, name, pwHash, salt")
            .Values($"{user.Guid}, {user.Name}, {user.PwHash}, {user.Salt}");

        connection.Execute(builder.Sql, builder.Parameters);
        return userGuid;
    }

    public TokenLease? Login(Guid user, string password)
    {
        Builder sql = SimpleBuilder.Create($"SELECT guid, name, pwHash, salt from user WHERE guid = {user.ToString()}");
        User? result = connection.QueryFirstOrDefault<User>(sql.Sql, sql.Parameters);
        
        if (result is null)
            return null;
        
        if (!Crypto.VerifyHashedPassword(result.PwHash, result.Salt + password)) 
            return null;
        
        return tokenManager.GenerateTokenFor(result.Guid, result.Name);
    }

    public void Logout(Guid user)
    {
        //TODO
        //invalidate current token for user
    }

    public User? GetUser(string name)
    {
        Builder sql = SimpleBuilder.Create($"SELECT guid, name from user WHERE name = {name} LIMIT 1");
        return connection.QueryFirstOrDefault<User>(sql.Sql, sql.Parameters);
    }

    public User? GetUser(Guid guid)
    {
        Builder sql = SimpleBuilder.Create($"SELECT guid, name from user WHERE guid = {guid.ToString()} LIMIT 1");
        return connection.QueryFirstOrDefault<User>(sql.Sql, sql.Parameters);
    }
}