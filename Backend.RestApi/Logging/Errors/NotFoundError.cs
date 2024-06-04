using FluentResults;

namespace Backend.RestApi.Logging.Errors;

public class NotFoundError:Error
{
    public NotFoundError(object identifier)
        :base($"Item with identifier \"{identifier}\" was not found")
    {
        Metadata.Add("ErrorCode", "404");
    }
}