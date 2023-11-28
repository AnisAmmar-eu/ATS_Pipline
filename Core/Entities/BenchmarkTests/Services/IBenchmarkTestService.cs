using Core.Entities.BenchmarkTests.Models.DB;
using Core.Entities.BenchmarkTests.Models.DTO;
using Core.Shared.Paginations;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.BenchmarkTests.Services;

public interface IBenchmarkTestService : IBaseEntityService<BenchmarkTest, DTOBenchmarkTest>
{
	public Task<TimeSpan> GenerateRows(int nbItems);
}