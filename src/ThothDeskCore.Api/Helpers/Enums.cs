// ReSharper disable InconsistentNaming
namespace ThothDeskCore.Api.Helpers
{
    //TODO finish completing this enum
    public class Enums
    {
        /// <summary>
        /// Enums for all the Http codes
        /// <b>1xx: Informational Responses</b>
        /// <b>2xx: Success Codes</b>
        /// <b>3xx: Redirection codes</b>
        /// <b>4xx: Ckient Error Codes</b>
        /// <b>5xx: Server Error Codes</b>
        /// </summary>
        public enum HttpCodes
        {
            CONTINUE_100 = 100,
            SWITCHING_PROTOCOLS_101 = 101,
            PROCESSING_102 = 102,
            EARLY_HINTS = 103,

            OK_200 = 200,
            CREATED_201 = 201,
            ACCEPTED_202 = 202,
            NON_AUTHORITATIVE_INFORMATION_203 = 203,
            NO_CONTENT_204 = 204,
            RESET_CONTENT = 205,

            NOT_FOUD = 404,




        }
    }
}
