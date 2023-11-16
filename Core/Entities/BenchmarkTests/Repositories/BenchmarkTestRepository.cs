using Core.Entities.BenchmarkTests.Models.DB;
using Core.Entities.BenchmarkTests.Models.DTO;
using Core.Shared.Data;
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
		return await _context.BenchmarkTest.OrderByDescending(log => log.TS).Skip(start).Take(nbItems).ToListAsync();
	}

	public async Task<List<BenchmarkTest>> GetRangeForPagination(int nbItems, int lastID)
	{
		return await _context.BenchmarkTest.OrderByDescending(b => b.ID)
			.Where(b => lastID == 0 || b.ID < lastID)
			.Take(nbItems)
			.ToListAsync();
	}

	public async Task RemoveAll()
	{
		_context.BenchmarkTest.RemoveRange(_context.BenchmarkTest);
		await _context.SaveChangesAsync();
	}

	public int GetCount()
	{
		return _context.BenchmarkTest.Count();
	}
}