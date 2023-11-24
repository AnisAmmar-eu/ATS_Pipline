using Core.Entities.BenchmarkTests.Models.DB;
using Core.Entities.BenchmarkTests.Models.DTO;
using Core.Shared.Paginations;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.BenchmarkTests.Services;

public interface IBenchmarkTestService : IBaseEntityService<BenchmarkTest, DTOBenchmarkTest>
{
	public Task<List<TimeSpan>> StartTest(int nbItems);

	public Task<List<DTOBenchmarkTest>> GetRange(int nbItems, int lastID, Pagination pagination);
}