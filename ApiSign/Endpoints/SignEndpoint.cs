using Carter;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.SignMatch.Services;
using Core.Entities.User.Dictionaries;
using Core.Entities.User.Models.DB.Acts;
using Core.Entities.User.Models.DTO.Acts;
using Core.Entities.User.Models.DTO.Acts.ActEntities;
using Core.Entities.User.Services.Acts;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Services.Background;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace ApiSign.Endpoints
{
    public class SignEndpoint : BaseEndpoint, ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            RouteGroupBuilder group = app.MapGroup("apiSign").WithTags(nameof(SignEndpoint));

            group.MapGet("status", () => new ApiResponse().Status);
            group.MapGet("sign/{anodeID:string}", SignAnode);
        }

        private static Task<JsonHttpResult<ApiResponse>> SignAnode(
            [FromRoute] string anodeID,
            ISignServices signServices,
            ILogService logService,
            HttpContext httpContext)
        {
            return GenericEndpoint(() => signServices.SignAnode(anodeID), logService, httpContext);
        }
    }
}