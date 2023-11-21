using Core.Shared.Models.ApiResponses.ApiStatuses;

namespace Core.Shared.Models.ApiResponses;

public partial class ApiResponse
{
	public object? Result { get; set; }
	public ApiStatus Status { get; set; }
}