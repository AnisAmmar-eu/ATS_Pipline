using System.Diagnostics;
using Core.Entities.BenchmarkTests.Models.DB;
using Core.Entities.BenchmarkTests.Models.DB.CameraTests;
using Core.Entities.BenchmarkTests.Models.DTO;
using Core.Entities.BenchmarkTests.Repositories;
using Core.Shared.Dictionaries;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.BenchmarkTests.Services;

public class BenchmarkTestService :
	BaseEntityService<IBenchmarkTestRepository, BenchmarkTest, DTOBenchmarkTest>,
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

		int nbRows = _anodeUOW.BenchmarkTest.GetCount();
		if (nbRows < nbItems)
		{
			watch.Start();
			nbItems -= nbRows;
			DateTimeOffset now = DateTimeOffset.Now;
			await _anodeUOW.StartTransaction();
			for (int i = 0; i < nbItems - 10; ++i)
			{
				if (i % 100000 == 0 && i != 0)
				{
					_anodeUOW.Commit();
					await _anodeUOW.CommitTransaction();
					await _anodeUOW.StartTransaction();
				}

				await _anodeUOW.BenchmarkTest.Add(await GenerateTest(now, i));
			}

			for (int i = 0; i < 10; ++i)
				await _anodeUOW.BenchmarkTest.Add(await GenerateTest(now, i, status: 4));
			// BenchmarkTest test = GenerateTest(now, 0, "RandomRID");
			// await AnodeUOW.BenchmarkTest.Add(test);
			_anodeUOW.Commit();
			await _anodeUOW.CommitTransaction();
			watch.Stop();
		}
		else if (nbRows > nbItems)
		{
			int toRemove = nbRows - nbItems;
			_anodeUOW.BenchmarkTest.RemoveRange(await _anodeUOW.BenchmarkTest.OldGetRange(nbRows - toRemove, toRemove));
		}

		return watch.Elapsed;
	}

	public async Task<List<TimeSpan>> StartTest(int nbItems)
	{
		Stopwatch watch = new();
		List<TimeSpan> ans = new();
		int nbRows = _anodeUOW.BenchmarkTest.GetCount();
		if (nbRows < nbItems)
		{
			watch.Start();
			nbItems -= nbRows;
			DateTimeOffset now = DateTimeOffset.Now;
			await _anodeUOW.StartTransaction();
			for (int i = 0; i < nbItems; ++i)
				await _anodeUOW.BenchmarkTest.Add(await GenerateTest(now, i));
			// BenchmarkTest test = GenerateTest(now, 0, "RandomRID");
			// await AnodeUOW.BenchmarkTest.Add(test);
			_anodeUOW.Commit();
			await _anodeUOW.CommitTransaction();
			watch.Stop();
			ans.Add(watch.Elapsed);
		}
		else if (nbRows > nbItems)
		{
			int toRemove = nbRows - nbItems;
			_anodeUOW.BenchmarkTest.RemoveRange(await _anodeUOW.BenchmarkTest.OldGetRange(nbRows - toRemove, toRemove));
		}

		watch.Restart();
		await _anodeUOW.BenchmarkTest.GetAll([b => b.StationID == 3], withTracking: false);
		watch.Stop();
		ans.Add(watch.Elapsed);
		watch.Restart();
		await _anodeUOW.BenchmarkTest.GetAll([b => b.AnodeType == 1], withTracking: false);
		watch.Stop();
		ans.Add(watch.Elapsed);
		watch.Restart();
		BenchmarkTest test = await _anodeUOW.BenchmarkTest.GetByWithThrow([b => b.RID == "RandomRID"], withTracking: false);
		watch.Stop();
		ans.Add(watch.Elapsed);
		watch.Restart();
		await _anodeUOW.BenchmarkTest.GetById(test.ID, withTracking: false);
		watch.Stop();
		ans.Add(watch.Elapsed);

		return ans;
	}

	private async Task<BenchmarkTest> GenerateTest(DateTimeOffset now, int index, string? rid = null, int? status = null)
	{
		int stationID = _random.Next(1, 6);
		int cameraID = _random.Next(1, 3);
		CameraTest cam = await _anodeUOW.CameraTest.GetById(cameraID);
		status ??= _random.Next(1, 4);

		int anodeType = _random.Next(1, 3);
		DateTimeOffset ts = now.Subtract(TimeSpan.FromMinutes(index));
		rid ??= $"{ts.ToString(AnodeFormat.RIDFormat)}_{stationID.ToString()}_{cameraID.ToString()}_{anodeType.ToString()}";
		return await Task.FromResult(new BenchmarkTest {
			TS = ts,
			TSIndex = ts,
			StationID = stationID,
			CameraID = cameraID,
			Status = status.Value,
			AnodeType = anodeType,
			RID = rid,
			CameraTest = cam,
		});
	}
}