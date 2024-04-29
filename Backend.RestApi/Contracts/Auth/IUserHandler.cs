namespace RestApiBackend.Contracts.Auth;

public interface IUserHandler
{
    Guid? CreateUser(string name, string password);
    string? Login(Guid user, string password);
    void Logout(Guid user);
    Guid? GetUser(string name);

    string GetName(Guid guid);
}