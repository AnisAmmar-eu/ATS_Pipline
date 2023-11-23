using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace Core.Shared.Models.DTO.Kernel.Interfaces;

public interface IExtensionBinder<TSelf> where TSelf: IExtensionBinder<TSelf>
{
	/// <summary>
	/// The method discovered by RequestDelegateFactory on types used as parameters of route
	/// handler delegates to support custom binding.
	/// </summary>
	/// <returns>The value to assign to the parameter.</returns>
	static abstract ValueTask<TSelf?> BindAsync(HttpContext httpContext);
}