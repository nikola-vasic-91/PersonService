using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using static PersonService.API.Constants.OtherConstants;

namespace PersonService.API.Helpers
{
    /// <summary>
    /// Class used for applying the Correlation id header
    /// </summary>
    internal class SwaggerXCorrelationIdFilter : IOperationFilter
    {
        /// <summary>
        /// Apples the new header parameter
        /// </summary>
        /// <param name="operation">Api operation</param>
        /// <param name="context">Operation filter context</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            var schema = context.SchemaGenerator.GenerateSchema(typeof(Guid), context.SchemaRepository);
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = CorrelationIdHeaderName,
                In = ParameterLocation.Header,
                Description = "Correlates HTTP requests between a client and server",
                Schema = schema,
                Required = true,
            });
        }
    }
}
