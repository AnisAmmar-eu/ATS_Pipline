using System.Diagnostics;
using System.Linq.Expressions;
using Core.Entities.BenchmarkTests.Models.DB;
using Core.Entities.BenchmarkTests.Models.DTO;
using Core.Entities.BenchmarkTests.Repositories;
using Core.Shared.Dictionaries;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.BenchmarkTests.Services;

public class BenchmarkTestService : ServiceBaseEntity<IBenchmarkTestRepository, BenchmarkTest, DTOBenchmarkTest>, IBenchmarkTestService
{
	private readonly Random _random = new();
	
	public BenchmarkTestService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}

	public async Task<List<TimeSpan>> StartTest(int nbItems)
	{
		int nbRows = AnodeUOW.BenchmarkTest.GetCount();
		if (nbRows < nbItems)
		{
			nbItems = nbItems - nbRows - 1;
			DateTimeOffset now = DateTimeOffset.Now;
			await AnodeUOW.BenchmarkTest.RemoveAll();
			await AnodeUOW.StartTransaction();
			for (int i = 0; i < nbItems; ++i)
				await AnodeUOW.BenchmarkTest.Add(GenerateTest(now, i));
			BenchmarkTest test = GenerateTest(now, 0, "RandomRID");
			await AnodeUOW.BenchmarkTest.Add(test);
			AnodeUOW.Commit();
			await AnodeUOW.CommitTransaction();
		} else if (nbRows > nbItems)
		{
			int toRemove = nbRows - nbItems;
			AnodeUOW.BenchmarkTest.RemoveRange(await AnodeUOW.BenchmarkTest.GetRange(nbRows - toRemove, toRemove));
		}

		List<TimeSpan> ans = new();
		Stopwatch watch = new();
		watch.Start();
		await AnodeUOW.BenchmarkTest.GetAll(new Expression<Func<BenchmarkTest, bool>>[]
		{
			b => b.StationID == 3
		});
		watch.Stop();
		ans.Add(watch.Elapsed);
		watch.Restart();
		await AnodeUOW.BenchmarkTest.GetAll(new Expression<Func<BenchmarkTest, bool>>[]
		{
			b => b.AnodeType == AnodeTypeDict.D20
		});
		watch.Stop();
		ans.Add(watch.Elapsed);
		watch.Restart();
		await AnodeUOW.BenchmarkTest.GetBy(new Expression<Func<BenchmarkTest, bool>>[]
		{
			b => b.RID == "RandomRID"
		});
		watch.Stop();
		ans.Add(watch.Elapsed);
		watch.Restart();
		await AnodeUOW.BenchmarkTest.GetById(4212120);
		watch.Stop();
		ans.Add(watch.Elapsed);

		return ans;
	}

	private BenchmarkTest GenerateTest(DateTimeOffset now, int index, string? rid = null)
	{
		int stationID = _random.Next(1, 6);
		int cameraID = _random.Next(1, 3);
		string anodeType = _random.Next(1, 3) == 1 ? AnodeTypeDict.D20 : AnodeTypeDict.DX;
		DateTimeOffset ts = now.Subtract(TimeSpan.FromMinutes(index));
		rid ??= $"{ts.ToString(AnodeFormat.RIDFormat)}_{stationID}_{cameraID}_{anodeType}";
		return new BenchmarkTest()
		{
			TS = ts,
			StationID = stationID,
			CameraID = cameraID,
			AnodeType = anodeType,
			RID = rid
		};
	}
}