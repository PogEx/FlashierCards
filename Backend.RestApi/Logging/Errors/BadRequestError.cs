using FluentResults;

namespace Backend.RestApi.Logging.Errors;

public class BadRequestError: Error
{
    public BadRequestError(string msg) 
        : base(msg)
    {
        Metadata.Add("ErrorCode", "400");
    }
}