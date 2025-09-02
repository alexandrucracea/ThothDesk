

namespace ThothDeskCore.Domain;

#region Enums and co constants
/// <summary>
/// Enums for all the Http codes
/// <b>1xx: Informational Responses</b>
/// <b>2xx: Success Codes</b>
/// <b>3xx: Redirection codes</b>
/// <b>4xx: Client Error Codes</b>
/// <b>5xx: Server Error Codes</b>
/// </summary>
public enum HttpCodes
{
    Continue100 = 100,
    SwitchingProtocols101 = 101,
    Processing102 = 102,
    EarlyHints = 103,

    Ok200 = 200,
    Created201 = 201,
    Accepted202 = 202,
    NonAuthoritativeInformation203 = 203,
    NoContent204 = 204,
    ResetContent = 205,

    NotFoud = 404,

    InternalServerError500 = 500
}


#endregion

public abstract class AppException : Exception
{
    public HttpCodes ErrorCode { get; }

    protected AppException(string errorMessage, HttpCodes errorCode) : base(errorMessage)
    {
        ErrorCode = errorCode;
    }
}

public sealed class NotFoundException : AppException
{
    public NotFoundException(string resourceName) : base($"{resourceName} was not found", HttpCodes.NotFoud)
    {
    }
}

