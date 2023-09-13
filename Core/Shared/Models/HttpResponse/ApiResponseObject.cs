using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Core.Shared.Models.HttpResponse;

public class ApiResponseObject
{
    // With default statusCode
    public ApiResponseObject()
    {
        Status = new ApiStatusObject { Code = 200 };
    }

    public ApiResponseObject(object? result)
    {
        Result = result;
        Status = new ApiStatusObject { Code = 200 };
    }

    public ApiResponseObject(string message)
    {
        Status = new ApiStatusObject { Code = 200, Message = message };
    }

    public ApiResponseObject(string message, object? result)
    {
        Result = result;
        Status = new ApiStatusObject { Code = 200, Message = message };
    }

    // With statusCode
    public ApiResponseObject(int code)
    {
        Status = new ApiStatusObject { Code = code };
    }

    public ApiResponseObject(int code, object? result)
    {
        Result = result;
        Status = new ApiStatusObject { Code = code };
    }

    public ApiResponseObject(int code, string message)
    {
        Status = new ApiStatusObject { Code = code, Message = message };
    }

    public ApiResponseObject(int code, string message, object? result)
    {
        Result = result;
        Status = new ApiStatusObject { Code = code, Message = message };
    }

    public object? Result { get; set; }
    public ApiStatusObject Status { get; set; }

    /*
    public JsonResult JsonResult()
    {
        return new JsonResult(this);
    }
    */

    public ObjectResult SuccessResult()
    {
        var toto = MethodBase.GetCurrentMethod()?.Name;
        Status.Code = 200;
        return new ObjectResult(this)
        {
            StatusCode = Status.Code
        };
    }

    public BadRequestObjectResult BadRequestResult()
    {
        Status.Code = 400;
        return new BadRequestObjectResult(this);
    }

    public ObjectResult ErrorResult()
    {
        return new ObjectResult(this)
        {
            StatusCode = Status.Code
        };
    }
    /*

    public async Task<JsonResult> SuccessResult(ILogsService logsService, ControllerContext context)
    {
        await CreateLog(logsService, context);

        return new JsonResult(this);
    }

    public async Task<ObjectResult> ErrorResult(ILogsService logsService, ControllerContext context, Exception e)
    {
        switch (e)
        {
            case ArgumentException:
                Status.Code = 400;
                Result = e.Message;
                break;
            case UnauthorizedAccessException:
                Status.Code = 401;
                Result = "Unauthorized action: " + e.Message;
                break;
            case EntityNotFoundException:
                Status.Code = 404;
                Result = e.Message;
                break;
            case TimeoutException:
                Status.Code = 408;
                Result = "Request timeout: " + e.Message;
                break;
            case InvalidOperationException:
                Status.Code = 409;
                Result = "Request cause a conflict: " + e.Message;
                break;
            case NotImplementedException:
                Status.Code = 501;
                Result = "Not implemented request: " + e.Message;
                break;
            default:
                Status.Code = 500;
                Result = "Internal Server Error: " + e.Message;
                break;
        }

        await CreateLog(logsService, context);

        return new ObjectResult(Result)
        {
            StatusCode = Status.Code
        };
    }

    private async Task CreateLog(ILogsService logsService, ControllerContext context)
    {
        try
        {
            await logsService.Create(
                DateTimeOffset.Now,
                Dns.GetHostName(),
                context.ActionDescriptor.ControllerTypeInfo.Assembly.ManifestModule.Name,
                context.ActionDescriptor.ControllerTypeInfo.Name,
                context.ActionDescriptor.ActionName,
                context.ActionDescriptor.AttributeRouteInfo?.Template ?? "",
                Status.Code,
                JsonConvert.SerializeObject(Result)
            );
        }
        catch
        {
        }
    }
    */
}