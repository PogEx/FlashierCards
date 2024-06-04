using FluentResults;

namespace Backend.RestApi.Logging.Errors;

public class DuplicateItemError: Error
{
    public DuplicateItemError(string identifier) 
        : base($"Item with \"{identifier}\" already exists")
    {
        Metadata.Add("ErrorCode", "409");
    }
}