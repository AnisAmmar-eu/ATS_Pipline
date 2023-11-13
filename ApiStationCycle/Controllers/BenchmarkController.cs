using Core.Entities.BenchmarkTests.Services;
using Core.Shared.Models.HttpResponse;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Mvc;

namespace ApiStationCycle.Controllers;

[ApiController]
[Route("benchmark")]
public class BenchmarkController : ControllerBase
{
	private IBenchmarkTestService _benchmarkTestService;
	private ILogService _logService;

	public BenchmarkController(IBenchmarkTestService benchmarkTestService, ILogService logService)
	{
		_benchmarkTestService = benchmarkTestService;
		_logService = logService;
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