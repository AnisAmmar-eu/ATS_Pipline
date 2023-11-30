using System.Diagnostics;
using System.Linq.Expressions;
using Core.Entities.BenchmarkTests.Models.DB;
using Core.Entities.BenchmarkTests.Models.DTO;
using Core.Entities.BenchmarkTests.Repositories;
using Core.Shared.Dictionaries;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.BenchmarkTests.Services;

public class BenchmarkTestService : BaseEntityService<IBenchmarkTestRepository, BenchmarkTest, DTOBenchmarkTest>,
	IBenchmarkTestService
{
	private readonly Random _random = new();

	public BenchmarkTestService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}

	public async Task<TimeSpan> GenerateRows(int nbItems)
	{
		Stopwatch watch = new();
		if (nbItems < 10)
			throw new ArgumentException("Not enough items");

		int nbRows = AnodeUOW.BenchmarkTest.GetCount();
		if (nbRows < nbItems)
		{
			watch.Start();
			nbItems -= nbRows;
			DateTimeOffset now = DateTimeOffset.Now;
			await AnodeUOW.StartTransaction();
			for (int i = 0; i < nbItems - 10; ++i)
				await AnodeUOW.BenchmarkTest.Add(await GenerateTest(now, i));

			for (int i = 0; i < 10; ++i)
				await AnodeUOW.BenchmarkTest.Add(await GenerateTest(now, i, status: 4));
			// BenchmarkTest test = GenerateTest(now, 0, "RandomRID");
			// await AnodeUOW.BenchmarkTest.Add(test);
			AnodeUOW.Commit();
			await AnodeUOW.CommitTransaction();
			watch.Stop();
		}
		else if (nbRows > nbItems)
		{
			int toRemove = nbRows - nbItems;
			AnodeUOW.BenchmarkTest.RemoveRange(await AnodeUOW.BenchmarkTest.OldGetRange(nbRows - toRemove, toRemove));
		}

		return watch.Elapsed;
	}

	public async Task<List<TimeSpan>> StartTest(int nbItems)
	{
		Stopwatch watch = new();
		List<TimeSpan> ans = new();
		int nbRows = AnodeUOW.BenchmarkTest.GetCount();
		if (nbRows < nbItems)
		{
			watch.Start();
			nbItems -= nbRows;
			DateTimeOffset now = DateTimeOffset.Now;
			await AnodeUOW.StartTransaction();
			for (int i = 0; i < nbItems; ++i)
				await AnodeUOW.BenchmarkTest.Add(await GenerateTest(now, i));
			// BenchmarkTest test = GenerateTest(now, 0, "RandomRID");
			// await AnodeUOW.BenchmarkTest.Add(test);
			AnodeUOW.Commit();
			await AnodeUOW.CommitTransaction();
			watch.Stop();
			ans.Add(watch.Elapsed);
		}
		else if (nbRows > nbItems)
		{
			int toRemove = nbRows - nbItems;
			AnodeUOW.BenchmarkTest.RemoveRange(await AnodeUOW.BenchmarkTest.OldGetRange(nbRows - toRemove, toRemove));
		}

		watch.Restart();
		await AnodeUOW.BenchmarkTest
			.GetAll( new Expression<Func<BenchmarkTest, bool>>[] { b => b.StationID == 3 }, withTracking: false);
		watch.Stop();
		ans.Add(watch.Elapsed);
		watch.Restart();
		await AnodeUOW.BenchmarkTest
			.GetAll( new Expression<Func<BenchmarkTest, bool>>[] { b => b.AnodeType == 1 }, withTracking: false);
		watch.Stop();
		ans.Add(watch.Elapsed);
		watch.Restart();
		BenchmarkTest test = await AnodeUOW.BenchmarkTest.GetBy(
			new Expression<Func<BenchmarkTest, bool>>[] {
				b => b.RID == "RandomRID"
				},
			withTracking: false);
		watch.Stop();
		ans.Add(watch.Elapsed);
		watch.Restart();
		await AnodeUOW.BenchmarkTest.GetById(test.ID, withTracking: false);
		watch.Stop();
		ans.Add(watch.Elapsed);

		return ans;
	}

	private Task<BenchmarkTest> GenerateTest(DateTimeOffset now, int index, string? rid = null, int? status = null)
	{
		int stationID = _random.Next(1, 6);
		int cameraID = _random.Next(1, 3);
		status ??= _random.Next(1, 4);

		int anodeType = _random.Next(1, 3);
		DateTimeOffset ts = now.Subtract(TimeSpan.FromMinutes(index));
		rid ??= $"{ts.ToString(AnodeFormat.RIDFormat)}_{stationID.ToString()}_{cameraID.ToString()}_{anodeType.ToString()}";
		return Task.FromResult(new BenchmarkTest {
			TS = ts,
			StationID = stationID,
			CameraID = cameraID,
			Status = status.Value,
			AnodeType = anodeType,
			RID = rid,
		});
	}
}