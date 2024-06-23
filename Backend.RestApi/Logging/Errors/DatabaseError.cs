using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Backend.RestApi.Logging.Errors;

public class DatabaseError: Error
{
    public DatabaseError(DbUpdateException e):base("A database error has occured")
    {
        CausedBy(e);
        Metadata.Add("ErrorCode", "503");
    }
}