using Core.Entities.BenchmarkTests.Models.DB;
using Core.Entities.BenchmarkTests.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Paginations;
using Core.Shared.Paginations.Filtering;
using Core.Shared.Paginations.Sorting;
using Core.Shared.Repositories.Kernel;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities.BenchmarkTests.Repositories;

public class BenchmarkTestRepository : RepositoryBaseEntity<AnodeCTX, BenchmarkTest, DTOBenchmarkTest>,
	IBenchmarkTestRepository
{
	public BenchmarkTestRepository(AnodeCTX context) : base(context)
	{
	}

	public async Task<List<BenchmarkTest>> OldGetRange(int start, int nbItems)
	{
		return await Context.BenchmarkTest.OrderByDescending(log => log.TS).Skip(start).Take(nbItems).ToListAsync();
	}

	public async Task<List<BenchmarkTest>> GetRangeForPagination(int nbItems, int lastID, Pagination pagination)
	{
		return await Context.BenchmarkTest.Include("CameraTest").AsNoTracking()
			.FilterFromPagination<BenchmarkTest, DTOBenchmarkTest>(pagination, lastID) // Custom function in filter.
			.SortFromPagination<BenchmarkTest, DTOBenchmarkTest>(pagination) // Custom function in Sort.
			.Take(nbItems)
			.ToListAsync();
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