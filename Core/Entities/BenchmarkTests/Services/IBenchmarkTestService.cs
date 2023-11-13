using Core.Entities.BenchmarkTests.Models.DB;
using Core.Entities.BenchmarkTests.Models.DTO;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.BenchmarkTests.Services;

public interface IBenchmarkTestService : IServiceBaseEntity<BenchmarkTest, DTOBenchmarkTest>
{
	public Task<List<TimeSpan>> StartTest(int nbItems);
}