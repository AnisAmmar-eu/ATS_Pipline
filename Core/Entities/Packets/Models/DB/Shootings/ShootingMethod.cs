using System.Globalization;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Core.Entities.IOT.Dictionaries;
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
		AnodeType = dtoShooting.AnodeType;
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
		return new(this);
	}

	public FileInfo GetImagePathFromRoot(int stationID, string root, string anodeType, int camera)
	{
		// TODO Should use RID instead of ShootingTS
		string filename
			= $"S{stationID:00}T{anodeType}C{camera:00}T{ShootingTS.ToString(AnodeFormat.RIDFormat)}.jpg";
		string path
			= $@"S{stationID:00}\T{anodeType}\Y{ShootingTS.Year.ToString()}\M{ShootingTS.Month:00}\D{
				ShootingTS.Day:00}\C{camera:00}\";
		return new($@"{root}\{path}\{filename}");
	}

	[GeneratedRegex("S(?<stationID>[0-9]{2})T(?<anodeType>.*)C(?<camera>[0-9]{2})T(?<TS>.*).jpg")]
	private static partial Regex FilenameRegex();

	public static FileInfo GetImagePathFromFilename(string root, string filename)
	{
		Regex regex = FilenameRegex();
		GroupCollection groups = regex.Match(filename).Groups;
		DateTimeOffset date = DateTimeOffset.ParseExact(
			groups["TS"].Value,
			AnodeFormat.RIDFormat,
			CultureInfo.InvariantCulture.DateTimeFormat);
		string path
			= $@"S{groups["stationID"].Value}\T{groups["anodeType"]}\Y{date.Year.ToString()}\M{date.Month:00}\D{
				date.Day:00}\C{groups["camera"]}\";
		return new($@"{root}\{path}\{filename}");
	}

	public async Task SendImages(string imagesPath)
	{
		MultipartFormDataContent formData = new();
		formData.Headers.ContentType!.MediaType = "multipart/form-data";
		for (int i = 1; i <= 2; ++i)
		{
			FileInfo image
				= GetImagePathFromRoot(Station.ID, imagesPath, AnodeType, i);
			if (!image.Exists)
				continue;

			StreamContent content = new(File.Open(image.FullName, FileMode.Open));
			content.Headers.ContentType = new("image/jpeg");
			formData.Add(content, image.Name, image.Name);
		}

		if (!formData.Any())
			return;

		using HttpClient httpClient = new();
		httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));

		HttpResponseMessage response
			= await httpClient.PostAsync($"{ITApisDict.ServerReceiveAddress}/apiServerReceive/images", formData);
		if (!response.IsSuccessStatusCode)
			throw new HttpRequestException("Could not send images to the server: " + response.ReasonPhrase);
	}

	protected override Task InheritedBuild(IAnodeUOW anodeUOW)
	{
		DirectoryInfo directory1 = new(ShootingUtils.Camera1);
		DirectoryInfo directory2 = new(ShootingUtils.Camera2);
		DirectoryInfo archive1 = new(ShootingUtils.Archive1);
		DirectoryInfo archive2 = new(ShootingUtils.Archive2);
		if (!directory1.Exists || !directory2.Exists || !archive1.Exists || !archive2.Exists)
			throw new IOException("One or more ShootingFolders do not exist");
		// First and Third hole because second hole isn't accounted for
		FileInfo? firstHole = null;
		FileInfo? thirdHole = null;
		DateTimeOffset? tsFirstImage = null;
		DateTimeOffset startAssign = DateTimeOffset.Now;
		while ((firstHole is null || thirdHole is null) && DateTimeOffset.Now - startAssign <= TimeSpan.FromSeconds(30))
		{
			if (firstHole is null)
			{
				firstHole = GetImageInDirectory(directory1, StationCycleRID);
				if (firstHole is not null)
				{
					DateTimeOffset tsHoleImage
						= DateTimeOffset.ParseExact(
							ExtractTSFromName(firstHole.Name, StationCycleRID),
							AnodeFormat.RIDFormat,
							CultureInfo.InvariantCulture.DateTimeFormat);
					tsFirstImage = (tsFirstImage is null || tsHoleImage < tsFirstImage) ? tsHoleImage : tsFirstImage;
				}
			}

			if (thirdHole is null)
			{
				thirdHole = GetImageInDirectory(directory2, StationCycleRID);
				if (thirdHole is not null)
				{
					DateTimeOffset tsHoleImage
						= DateTimeOffset.ParseExact(
							ExtractTSFromName(thirdHole.Name, StationCycleRID),
							AnodeFormat.RIDFormat,
							CultureInfo.InvariantCulture.DateTimeFormat);
					tsFirstImage = (tsFirstImage is null || tsHoleImage < tsFirstImage) ? tsHoleImage : tsFirstImage;
				}
			}
		}

		if (tsFirstImage is null)
			throw new("tsFirstImage should NOT be null");

		UpdatePacketAndStationCycle(anodeUOW, firstHole, thirdHole, tsFirstImage.Value);
		return Task.CompletedTask;
		// DetectionPacket is now dequeued by the ADS Notification service.
		// Task task2 = DequeueDetectionPacket();
		// await task2;
	}

	private void UpdatePacketAndStationCycle(
		IAnodeUOW anodeUOW,
		FileInfo? firstHole,
		FileInfo? thirdHole,
		DateTimeOffset tsFirstImage)
	{
		// StationCycle is already set
		if (StationCycle is null)
			throw new ArgumentException("Station Cycle should NOT be null when building a ShootingPacket");

		StationCycle.ShootingPacket = this;
		StationCycle.ShootingID = ID;
		// ?. => If firstHole not null then...
		while (IsFileLocked(firstHole) || IsFileLocked(thirdHole))
		{
		}

		if (firstHole is not null)
			SaveImageAndThumbnail(firstHole, ImagePath, ThumbnailPath, StationCycle.AnodeType, tsFirstImage, 1);

		if (thirdHole is not null)
			SaveImageAndThumbnail(thirdHole, ImagePath, ThumbnailPath, StationCycle.AnodeType, tsFirstImage, 2);

		Status = PacketStatus.Completed;
		ShootingTS = tsFirstImage;
		HasError = firstHole is null || thirdHole is null;
		StationCycle.ShootingStatus = Status;
		anodeUOW.StationCycle.Update(StationCycle);
	}

	private static void SaveImageAndThumbnail(
		FileInfo file,
		string imageRoot,
		string thumbnailRoot,
		string anodeType,
		DateTimeOffset date,
		int camera)
	{
		string filename = $"S{Station.ID:00}T{anodeType}C{camera:00}T{date.ToString(AnodeFormat.RIDFormat)}.jpg";
		string path
			= $@"S{Station.ID:00}\T{anodeType}\Y{date.Year.ToString()}\M{date.Month:00}\D{date.Day:00}\C{camera:00}\";
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
			.FindAll(fileInfo => rid.Length == 0 || ExtractRIDFromName(fileInfo.Name) == rid);
		images.Sort((x, y) => DateTime.Compare(x.CreationTime, y.CreationTime));
		return (images.Count == 0) ? null : images[0];
	}

	private static bool IsFileLocked(FileInfo? file)
	{
		if (file is null)
			return false;

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
		if (string.IsNullOrWhiteSpace(fileName))
			return string.Empty;
		// X_ is the station number
		int charLocation = "X_".Length + AnodeFormat.RIDFormat.Length;
		return (charLocation > 0) ? fileName[..charLocation] : string.Empty;
	}

	private static string ExtractTSFromName(string fileName, string? rid)
	{
		if (rid is null || string.IsNullOrWhiteSpace(fileName))
			return string.Empty;

		int charLocation = fileName.IndexOf('.');
		charLocation = ((charLocation == 0) ? fileName.Length : charLocation) - rid.Length - 1;
		return fileName.Substring(rid.Length + 1, charLocation);
	}
}