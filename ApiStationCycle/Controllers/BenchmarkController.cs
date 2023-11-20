using Core.Entities.BenchmarkTests.Models.DTO;
using Core.Entities.BenchmarkTests.Services;
using Core.Shared.Models.HttpResponse;
using Core.Shared.Pagination;
using Core.Shared.Pagination.Filtering;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Mvc;

namespace ApiStationCycle.Controllers;

[ApiController]
[Route("benchmark")]
public class BenchmarkController : ControllerBase
{
	private readonly IBenchmarkTestService _benchmarkTestService;
	private readonly ILogService _logService;

	public BenchmarkController(IBenchmarkTestService benchmarkTestService, ILogService logService)
	{
		_benchmarkTestService = benchmarkTestService;
		_logService = logService;
	}

	[HttpPut("range/{nbOfItems}/{lastID}")]
	public async Task<IActionResult> GetRange([FromRoute] int nbOfItems, [FromRoute] int lastID,
		[FromBody] Pagination pagination)
	{
		List<DTOBenchmarkTest> res;
		try
		{
			res = await _benchmarkTestService.GetRange(nbOfItems, lastID, pagination);
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ApiResponseObject(res).SuccessResult(_logService, ControllerContext);
	}

	[HttpGet("{nbOfItems}")]
	public async Task<IActionResult> Benchmark([FromRoute] int nbOfItems)
	{
		List<TimeSpan> ans;
		try
		{
			ans = await _benchmarkTestService.StartTest(nbOfItems);
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ApiResponseObject(ans).SuccessResult(_logService, ControllerContext);
	}
}