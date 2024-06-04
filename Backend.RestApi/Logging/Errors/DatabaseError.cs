using FluentResults;

namespace Backend.RestApi.Logging.Errors;

public class DatabaseError:Error
{
    public DatabaseError():base("A database error has occured")
    {
        Metadata.Add("ErrorCode", "503");
    }
}