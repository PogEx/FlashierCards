using FluentResults;

namespace Backend.RestApi.Logging.Errors;

public class ForbiddenError: Error
{
    public ForbiddenError(Guid user, Guid item)
        :base($"User with id \"{user}\" has no permission to access item with id \"{item}\"") 
    {
        Metadata.Add("ErrorCode", "403");
    }
}