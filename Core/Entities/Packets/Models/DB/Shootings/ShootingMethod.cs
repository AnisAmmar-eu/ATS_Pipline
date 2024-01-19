using System.Globalization;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Core.Entities.IOT.Dictionaries;
using Core.Entities.Packets.Models.DTO.Shootings;
using Core.Entities.Packets.Models.Structs;
using Core.Shared.Dictionaries;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Packets.Models.DB.Shootings;

public partial class Shooting
{
	public Shooting()
	{
	}

	public Shooting(DTOShooting dtoShooting) : base(dtoShooting)
	{
		ShootingTS = dtoShooting.ShootingTS;
		SyncIndex = dtoShooting.SyncIndex;
		AnodeType = dtoShooting.AnodeType;
		AnodeSize = dtoShooting.AnodeSize;
		Cam01Status = dtoShooting.Cam01Status;
		Cam02Status = dtoShooting.Cam02Status;
	}

	public Shooting(ShootingStruct adsStruct)
	{
		StationCycleRID = adsStruct.CycleRID.ToRID();
		ShootingTS = adsStruct.TS.GetTimestamp();
		SyncIndex = adsStruct.SyncIndex;
		AnodeType = AnodeTypeDict.AnodeTypeIntToString(adsStruct.AnodeType);
		AnodeSize = adsStruct.AnodeSize;
		Cam01Status = adsStruct.Cam01Status;
		Cam02Status = adsStruct.Cam02Status;
	}

	public override DTOShooting ToDTO()
	{
		return new(this);
	}

	public static FileInfo GetImagePathFromRoot(
		string rid,
		int stationID,
		string root,
		string anodeType,
		int camera,
		string extension)
	{
		DateTimeOffset ridTS
			= DateTimeOffset.ParseExact(rid[2..], AnodeFormat.RIDFormat, CultureInfo.InvariantCulture);
		string filename
			= $"S{stationID:00}T{anodeType}C{camera:00}T{rid[2..]}.{extension}";
		string path
			= $@"S{stationID:00}\T{anodeType}\Y{ridTS.Year.ToString()}\M{ridTS.Month:00}\D{
				ridTS.Day:00}\C{camera:00}\";
		return new($@"{root}\{path}\{filename}");
	}

	[GeneratedRegex("S(?<stationID>[0-9]{2})T(?<anodeType>.*)C(?<camera>[0-9]{2})T(?<TS>.*)\\..*")]
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

	public async Task SendImages(string imagesPath, string extension)
	{
		MultipartFormDataContent formData = new();
		formData.Headers.ContentType!.MediaType = "multipart/form-data";
		for (int i = 1; i <= 2; ++i)
		{
			FileInfo image
				= GetImagePathFromRoot(StationCycleRID, Station.ID, imagesPath, AnodeType, i, extension);
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
		if (Cam01Status == 1)
		{
			FileInfo image = GetImagePathFromRoot(StationCycleRID, Station.ID, ImagesPath, AnodeType, 1, Extension);
			HasFirstShoot = image.Exists;
			HasError = HasError || !HasFirstShoot;
		}

		if (Cam02Status == 1)
		{
			FileInfo image = GetImagePathFromRoot(StationCycleRID, Station.ID, ImagesPath, AnodeType, 2, Extension);
			HasSecondShoot = image.Exists;
			HasError = HasError || !HasSecondShoot;
		}

		return Task.CompletedTask;
	}
}