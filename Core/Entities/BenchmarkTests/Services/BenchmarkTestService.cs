using System.Diagnostics;
using System.Linq.Expressions;
using Core.Entities.BenchmarkTests.Models.DB;
using Core.Entities.BenchmarkTests.Models.DB.CameraTests;
using Core.Entities.BenchmarkTests.Models.DTO;
using Core.Entities.BenchmarkTests.Repositories;
using Core.Shared.Dictionaries;
using Core.Shared.Paginations;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.BenchmarkTests.Services;

public class BenchmarkTestService : ServiceBaseEntity<IBenchmarkTestRepository, BenchmarkTest, DTOBenchmarkTest>,
	IBenchmarkTestService
{
	private readonly Random _random = new();
	private static CameraTest? _cam1 = null;
	private static CameraTest? _cam2 = null;

	public BenchmarkTestService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}

	public async Task<List<TimeSpan>> StartTest(int nbItems)
	{
		Stopwatch watch = new();
		List<TimeSpan> ans = new();
		int nbRows = AnodeUOW.BenchmarkTest.GetCount();
		if (nbRows < nbItems)
		{
			watch.Start();
			nbItems = nbItems - nbRows;
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
		await AnodeUOW.BenchmarkTest.GetAll(new Expression<Func<BenchmarkTest, bool>>[]
		{
			b => b.StationID == 3
		}, withTracking: false, includes: "CameraTest");
		watch.Stop();
		ans.Add(watch.Elapsed);
		watch.Restart();
		await AnodeUOW.BenchmarkTest.GetAll(new Expression<Func<BenchmarkTest, bool>>[]
		{
			b => b.AnodeType == AnodeTypeDict.D20
		}, withTracking: false, includes: "CameraTest");
		watch.Stop();
		ans.Add(watch.Elapsed);
		watch.Restart();
		BenchmarkTest test = await AnodeUOW.BenchmarkTest.GetBy(new Expression<Func<BenchmarkTest, bool>>[]
		{
			b => b.RID == "RandomRID"
		}, withTracking: false, includes: "CameraTest");
		watch.Stop();
		ans.Add(watch.Elapsed);
		watch.Restart();
		await AnodeUOW.BenchmarkTest.GetById(test.ID, withTracking: false, includes: "CameraTest");
		watch.Stop();
		ans.Add(watch.Elapsed);

		return ans;
	}

	public async Task<List<DTOBenchmarkTest>> GetRange(int nbItems, int lastID, Pagination pagination)

	{
		return (await AnodeUOW.BenchmarkTest.GetRangeForPagination(nbItems, lastID, pagination)).ConvertAll(b =>
			b.ToDTO());
	}

	private async Task<BenchmarkTest> GenerateTest(DateTimeOffset now, int index, string? rid = null)
	{
		int stationID = _random.Next(1, 6);
		int cameraID = _random.Next(1, 3);
		CameraTest cam;
		if (cameraID == 1)
		{
			_cam1 ??= await AnodeUOW.CameraTest.GetById(1);
			cam = _cam1;
		}
		else
		{
			_cam2 ??= await AnodeUOW.CameraTest.GetById(2);
			cam = _cam2;
		}

		string anodeType = _random.Next(1, 3) == 1 ? AnodeTypeDict.D20 : AnodeTypeDict.DX;
		DateTimeOffset ts = now.Subtract(TimeSpan.FromMinutes(index));
		rid ??= $"{ts.ToString(AnodeFormat.RIDFormat)}_{stationID}_{cameraID}_{anodeType}";
		return new BenchmarkTest
		{
			TS = ts,
			StationID = stationID,
			CameraID = cameraID,
			CameraTest = cam,
			AnodeType = anodeType,
			RID = rid
		};
	}
}