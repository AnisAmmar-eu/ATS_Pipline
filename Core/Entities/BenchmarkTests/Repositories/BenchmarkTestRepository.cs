using Core.Entities.BenchmarkTests.Models.DB;
using Core.Entities.BenchmarkTests.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities.BenchmarkTests.Repositories;

public class BenchmarkTestRepository : BaseEntityRepository<AnodeCTX, BenchmarkTest, DTOBenchmarkTest>,
	IBenchmarkTestRepository
{
	public BenchmarkTestRepository(AnodeCTX context) : base(context)
	{
	}

	public Task<List<BenchmarkTest>> OldGetRange(int start, int nbItems)
	{
		return Context.BenchmarkTest.OrderByDescending(log => log.TS).Skip(start).Take(nbItems).ToListAsync();
	}

	public async Task RemoveAll()
	{
		Context.BenchmarkTest.RemoveRange(Context.BenchmarkTest);
		await Context.SaveChangesAsync();
	}

	public int GetCount()
	{
		return Context.BenchmarkTest.Count();
	}
}