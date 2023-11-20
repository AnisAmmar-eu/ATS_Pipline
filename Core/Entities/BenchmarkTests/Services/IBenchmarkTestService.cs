using Core.Entities.BenchmarkTests.Models.DB;
using Core.Entities.BenchmarkTests.Models.DTO;
using Core.Shared.Pagination;
using Core.Shared.Pagination.Filtering;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.BenchmarkTests.Services;

public interface IBenchmarkTestService : IServiceBaseEntity<BenchmarkTest, DTOBenchmarkTest>
{
	public Task<List<TimeSpan>> StartTest(int nbItems);

	public Task<List<DTOBenchmarkTest>> GetRange(int nbItems, int lastID, Pagination pagination);
}