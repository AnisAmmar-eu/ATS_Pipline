using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Core.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Core.Shared.Models.ApiResponses;

public partial class ApiResponse
{
	/// <summary>
	/// Default JsonOptions which should be used when sending data to the front or for back to back communications.
	/// </summary>
	public static readonly JsonSerializerOptions JsonOptions = new() {
		PropertyNamingPolicy = null,
		TypeInfoResolver = new DefaultJsonTypeInfoResolver { Modifiers = { AddNestedDerivedTypes } },
	};

	public ApiResponse()
	{
		Status = new() { Code = 200 };
	}

	public ApiResponse(object? result)
	{
		Result = result;
		Status = new() { Code = 200 };
	}

	public ApiResponse(string message)
	{
		Status = new() { Code = 200, Message = message };
	}

	public ApiResponse(string message, object? result)
	{
		Result = result;
		Status = new() { Code = 200, Message = message };
	}

	public ApiResponse(int code)
	{
		Status = new() { Code = code };
	}

	public ApiResponse(int code, object? result)
	{
		Result = result;
		Status = new() { Code = code };
	}

	public ApiResponse(int code, string message)
	{
		Status = new() { Code = code, Message = message };
	}

	public ApiResponse(int code, string message, object? result)
	{
		Result = result;
		Status = new() { Code = code, Message = message };
	}

	public JsonHttpResult<ApiResponse> SuccessResult() => TypedResults.Json(this, JsonOptions);

	public JsonHttpResult<ApiResponse> SuccessResult(HttpContext httpContext)
	{
		CreateLog(httpContext);
		return TypedResults.Json(this, JsonOptions);
	}

	public JsonHttpResult<ApiResponse> ErrorResult(
		HttpContext httpContext,
		Exception e)
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

		CreateLog(httpContext);

		return TypedResults.Json(this, JsonOptions, statusCode: Status.Code);
	}

	private void CreateLog(HttpContext httpContext)
	{
		try
		{
			string result = JsonSerializer.Serialize(Result);
			result = result[..((result.Length > 50) ? 50 : result.Length)];

			if (Status.Code == 200)
			{
				Serilog.Log.ForContext("User", httpContext.User.Identity?.Name)
					.Information("[{code}]: {response}", Status.Code, result);
			}
			else if (Status.Code == 404)
			{
				Serilog.Log.ForContext("User", httpContext.User.Identity?.Name)
					.Warning("[{code}]: {response}", Status.Code, result);
			}
			else
			{
				Serilog.Log.ForContext("User", httpContext.User.Identity?.Name)
					.Error("[{code}]: {response}", Status.Code, result);
			}
		}
		catch
		{
			Serilog.Log.ForContext("User", httpContext.User.Identity?.Name)
				.Error("[{code}]: Error during response log.", Status.Code);
		}
	}

	/// <summary>
	/// This function allows for the class decorator <see cref="JsonDerivedType"/> to only specify direct children.
	/// The function then recursively adds the derived type of its children to the possible derived types.
	/// It allows for less cumbersome class decorators in DTO by needing to only specify direct children.
	/// </summary>
	/// <param name="jsonTypeInfo"></param>
	private static void AddNestedDerivedTypes(JsonTypeInfo jsonTypeInfo)
	{
		if (jsonTypeInfo.PolymorphismOptions is null)
			return;

		List<Type> derivedTypes = jsonTypeInfo.PolymorphismOptions.DerivedTypes
			.Where(t => Attribute.IsDefined(t.DerivedType, typeof(JsonDerivedTypeAttribute)))
			.Select(t => t.DerivedType)
			.ToList();
		HashSet<Type> hashset = new(derivedTypes);
		Queue<Type> queue = new(derivedTypes);
		while (queue.TryDequeue(out Type? derived))
		{
			if (!hashset.Contains(derived))
			{
				jsonTypeInfo.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(derived));
				hashset.Add(derived);
			}

			foreach (JsonDerivedTypeAttribute jsonDerivedTypeAttribute in derived
				.GetCustomAttributes<JsonDerivedTypeAttribute>())
			{
				queue.Enqueue(jsonDerivedTypeAttribute.DerivedType);
			}
		}
	}
}