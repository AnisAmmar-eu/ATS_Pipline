using System.Globalization;
using System.Text.RegularExpressions;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DTO.Shootings;
using Core.Entities.Packets.Models.Structs;
using Core.Shared.Dictionaries;
using Core.Shared.UnitOfWork.Interfaces;
using Stemmer.Cvb;

namespace Core.Entities.Packets.Models.DB.Shootings;

public partial class Shooting
{
	public Shooting()
	{
	}

	public Shooting(string imagePath, string thumbnailsPath)
	{
		ImagePath = imagePath;
		ThumbnailPath = thumbnailsPath;
	}

	public Shooting(DTOShooting dtoShooting) : base(dtoShooting)
	{
		AnodeIDKey = dtoShooting.AnodeIDKey;
		GlobalStationStatus = dtoShooting.GlobalStationStatus;
		LedStatus = dtoShooting.LedStatus;
		ProcedurePerformance = dtoShooting.ProcedurePerformance;
		ShootingTS = dtoShooting.ShootingTS;
	}

	public Shooting(ShootingStruct adsStruct)
	{
		StationCycleRID = adsStruct.StationCycleRID.ToRID();
		AnodeIDKey = adsStruct.AnodeIDKey;
		GlobalStationStatus = adsStruct.GlobalStationStatus;
		ProcedurePerformance = adsStruct.ProcedurePerformance;
		LedStatus = adsStruct.LedStatus;
		ShootingTS = adsStruct.ShootingTS.GetTimestamp();
	}

	public override DTOShooting ToDTO()
	{
		return new DTOShooting(this);
	}

	public FileInfo GetImagePathFromRoot(int stationID, string root, string anodeType, int camera)
	{
		string filename =
			$"S{stationID:00}T{anodeType}C{camera:00}T{ShootingTS.ToString(AnodeFormat.RIDFormat)}.jpg";
		string path =
			$@"S{stationID:00}\T{anodeType}\Y{ShootingTS.Year}\M{ShootingTS.Month:00}\D{ShootingTS.Day:00}\C{camera:00}\";
		return new FileInfo($@"{root}\{path}\{filename}");
	}

	public static FileInfo GetImagePathFromFilename(string root, string filename)
	{
		Regex regex = new("S(?<stationID>[0-9]{2})T(?<anodeType>.*)C(?<camera>[0-9]{2})T(?<TS>.*).jpg");
		GroupCollection groups = regex.Match(filename).Groups;
		DateTimeOffset date = DateTimeOffset.ParseExact(groups["TS"].Value, AnodeFormat.RIDFormat,
			CultureInfo.InvariantCulture.DateTimeFormat);
		string path =
			$@"S{groups["stationID"].Value}\T{groups["anodeType"]}\Y{date.Year}\M{date.Month:00}\D{date.Day:00}\C{groups["camera"]}\";
		return new FileInfo($@"{root}\{path}\{filename}");
	}

	protected override async Task InheritedBuild(IAnodeUOW anodeUOW)
	{
		DirectoryInfo directory1 = new(ShootingUtils.Camera1);
		DirectoryInfo directory2 = new(ShootingUtils.Camera2);
		DirectoryInfo archive1 = new(ShootingUtils.Archive1);
		DirectoryInfo archive2 = new(ShootingUtils.Archive2);
		if (!directory1.Exists || !directory2.Exists || !archive1.Exists || !archive2.Exists)
			throw new IOException("One or more ShootingFolders do not exist");
		// First and Third hole because second hole isn't taken into account.
		FileInfo? firstHole = null;
		FileInfo? thirdHole = null;
		DateTimeOffset? tsFirstImage = null;
		DateTimeOffset startAssign = DateTimeOffset.Now;
		while ((firstHole == null || thirdHole == null) && DateTimeOffset.Now - startAssign <= TimeSpan.FromSeconds(30))
		{
			if (firstHole == null)
			{
				firstHole = GetImageInDirectory(directory1, StationCycleRID);
				if (firstHole != null)
				{
					DateTimeOffset tsHoleImage =
						DateTimeOffset.ParseExact(ExtractTSFromName(firstHole.Name, StationCycleRID),
							AnodeFormat.RIDFormat,
							CultureInfo.InvariantCulture.DateTimeFormat);
					tsFirstImage = tsFirstImage == null || tsHoleImage < tsFirstImage ? tsHoleImage : tsFirstImage;
				}
			}

			if (thirdHole == null)
			{
				thirdHole = GetImageInDirectory(directory2, StationCycleRID);
				if (thirdHole != null)
				{
					DateTimeOffset tsHoleImage =
						DateTimeOffset.ParseExact(ExtractTSFromName(thirdHole.Name, StationCycleRID),
							AnodeFormat.RIDFormat, null);
					tsFirstImage = tsFirstImage == null || tsHoleImage < tsFirstImage ? tsHoleImage : tsFirstImage;
				}
			}
		}

		if (tsFirstImage == null)
			throw new Exception("tsFirstImage should NOT be null");

		await UpdatePacketAndStationCycle(anodeUOW, firstHole, thirdHole, tsFirstImage.Value);
		// DetectionPacket is now dequeued by the ADS Notification service.
		// Task task2 = DequeueDetectionPacket();
		// await task2;
	}

	private Task UpdatePacketAndStationCycle(IAnodeUOW anodeUOW, FileInfo? firstHole, FileInfo? thirdHole,
		DateTimeOffset tsFirstImage)
	{
		// StationCycle is already set
		if (StationCycle == null)
			throw new ArgumentException("Station Cycle should NOT be null when building a ShootingPacket");
		StationCycle.ShootingPacket = this;
		StationCycle.ShootingID = ID;
		// ?. => If firstHole not null then...
		while (IsFileLocked(firstHole) || IsFileLocked(thirdHole))
		{
		}

		if (firstHole != null)
			SaveImageAndThumbnail(firstHole, ImagePath, ThumbnailPath, StationCycle.AnodeType, tsFirstImage, 1);
		if (thirdHole != null)
			SaveImageAndThumbnail(thirdHole, ImagePath, ThumbnailPath, StationCycle.AnodeType, tsFirstImage, 2);

		Status = PacketStatus.Completed;
		ShootingTS = tsFirstImage;
		HasError = firstHole == null || thirdHole == null;
		StationCycle.ShootingStatus = Status;
		anodeUOW.StationCycle.Update(StationCycle);
		return Task.CompletedTask;
	}

	/*
	private static async Task DequeueDetectionPacket()
	{
		CancellationToken cancel = new();
		AdsClient tcClient = TwinCatConnectionManager.Connect(ADSUtils.AdsPort);
		uint removeHandle = tcClient.CreateVariableHandle(ADSUtils.DetectionRemove);
		await tcClient.WriteAnyAsync(removeHandle, true, cancel);
	}
	*/

	private static void SaveImageAndThumbnail(FileInfo file, string imageRoot, string thumbnailRoot, string anodeType,
		DateTimeOffset date, int camera)
	{
		string filename = $"S{Station.ID:00}T{anodeType}C{camera:00}T{date.ToString(AnodeFormat.RIDFormat)}.jpg";
		string path =
			$@"S{Station.ID:00}\T{anodeType}\Y{date.Year}\M{date.Month:00}\D{date.Day:00}\C{camera:00}\";
		string imagePath = $@"{imageRoot}\{path}";
		string thumbnailPath = $@"{thumbnailRoot}\{path}";
		Directory.CreateDirectory(imagePath);
		Directory.CreateDirectory(thumbnailPath);
		file.MoveTo($@"{imagePath}\{filename}");
		Image image = Image.FromFile(file.FullName);
		image.Save($@"{thumbnailPath}\{filename}", 0.2);
	}

	private static FileInfo? GetImageInDirectory(DirectoryInfo directory, string rid)
	{
		List<FileInfo> images = directory.EnumerateFiles().ToList()
			.FindAll(fileInfo => rid == string.Empty || ExtractRIDFromName(fileInfo.Name) == rid);
		images.Sort((x, y) => DateTime.Compare(x.CreationTime, y.CreationTime));
		return images.Count == 0 ? null : images[0];
	}

	private static bool IsFileLocked(FileInfo? file)
	{
		if (file == null) return false;
		try
		{
			using FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
			stream.Close();
		}
		catch (IOException)
		{
			return true;
		}

		return false;
	}

	private static string ExtractRIDFromName(string fileName)
	{
		if (string.IsNullOrWhiteSpace(fileName)) return string.Empty;
		// X_ is the station number
		int charLocation = "X_".Length + AnodeFormat.RIDFormat.Length;
		return charLocation > 0 ? fileName[..charLocation] : string.Empty;
	}

	private static string ExtractTSFromName(string fileName, string? rid)
	{
		if (rid == null || string.IsNullOrWhiteSpace(fileName)) return string.Empty;

		int charLocation = fileName.IndexOf(".", StringComparison.Ordinal);
		charLocation = (charLocation == 0 ? fileName.Length : charLocation) - rid.Length - 1;
		return fileName.Substring(rid.Length + 1, charLocation);
	}
}