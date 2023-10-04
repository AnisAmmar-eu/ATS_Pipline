using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ApiUser.SwaggerConfig
{
	/// <summary>
	/// Action header token
	/// </summary>
	public class SwaggerActionHeader : IOperationFilter
	{
		/// <summary>
		/// Apply function
		/// </summary>
		/// <param name="operation"></param>
		/// <param name="context"></param>
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			operation.Parameters ??= new List<OpenApiParameter>();

			operation.Parameters.Add(new OpenApiParameter
			{
				Name = "x-action-token",
				In = ParameterLocation.Header,
				Description = "Action token",
				Required = false
			});
		}
	}
}
