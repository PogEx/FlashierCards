using System.Web.Helpers;
using Backend.Common.Models;
using RestApiBackend.Contracts.Auth;

namespace RestApiBackend.Authentication;

public class UserHandler(ITokenManager tokenManager): IUserHandler
{
    public Guid? CreateUser(string name, string password)
    {
        string salt = Crypto.GenerateSalt();
        string hashedPassword = Crypto.HashPassword(salt + password);
            
        Guid userGuid = Guid.NewGuid();
        User user = new(userGuid, name, hashedPassword, salt);
        
        //save to database
        
        return userGuid;
    }

    public string Login(Guid user, string password)
    {
        //TODO
        //check if user exists here
        //Get Entry from database
        //prepend salt to pw
        //Hash Password
        //Compare Password Hash
        return tokenManager.Authenticate(GetName(user), password);
    }

    public void Logout(Guid user)
    {
        //TODO
        //invalidate current token for user
    }

    public Guid? GetUser(string name)
    {
        //TODO
        //Ask database for user existance
        //return guid of user

        return new Guid();
    }

    public string GetName(Guid guid)
    {
        //TODO
        //Get username of guid
        //return users name
        return "";
    }
}