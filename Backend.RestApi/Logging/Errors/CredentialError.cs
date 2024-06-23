using FluentResults;

namespace Backend.RestApi.Logging.Errors;

public class CredentialError: Error
{
    public CredentialError() : base("Username or password could not be found in the system!")
    {
        Metadata.Add("ErrorCode", "403");
    }
}