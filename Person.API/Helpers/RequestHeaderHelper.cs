using Microsoft.Extensions.Primitives;
using static PersonService.API.Constants.OtherConstants;

namespace PersonService.API.Helpers
{
    /// <summary>
    /// Helper class used for manipulating the request header data
    /// </summary>
    public static class RequestHeaderHelper
    {
        /// <summary>
        /// Gets the correlation id from the specific header
        /// </summary>
        /// <param name="header">Header value</param>
        /// <returns>Correlation id from the specific header</returns>
        public static Guid GetCorrelationId(this IHeaderDictionary header)
        {
            string key = CorrelationIdHeaderName;
            var correlationId = default(StringValues);

            if (header.ContainsKey(key))
            {
                header.TryGetValue(key, out correlationId);
            }

            return correlationId != default(StringValues) ? Guid.Parse(correlationId) : Guid.Empty;
        }
    }
}
