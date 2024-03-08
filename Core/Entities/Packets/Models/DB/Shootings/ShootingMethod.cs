using System.Globalization;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Core.Entities.IOT.Dictionaries;
using Core.Entities.Packets.Models.DTO.Shootings;
using Core.Entities.Packets.Models.Structs;
using Core.Shared.Dictionaries;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.Extensions.Logging;

namespace Core.Entities.Packets.Models.DB.Shootings;

public partial class Shooting
{
	public Shooting()
	{
	}

	public Shooting(DTOShooting dtoShooting) : base(dtoShooting)
	{
		ShootingTS = dtoShooting.ShootingTS;
		AnodeType = dtoShooting.AnodeType;
		Cam01Status = dtoShooting.Cam01Status;
		Cam02Status = dtoShooting.Cam02Status;
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
		try
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
		catch (Exception)
		{
			return new("undefined");
		}
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

	public async Task SendImages(string imagesPath, string extension, ILogger logger)
	{
		int cameraNumber = (Cam01Status == 1) ? 1 : 2;
		MultipartFormDataContent formData = new();
		formData.Headers.ContentType!.MediaType = "multipart/form-data";
		logger.LogInformation(
			"{StationCycleRID} {Station.ID} {imagesPath} {AnodeType} {i} {extension}",
			StationCycleRID,
			Station.ID,
			imagesPath,
			AnodeType,
			cameraNumber,
			extension);

			FileInfo image
				= GetImagePathFromRoot(StationCycleRID, Station.ID, imagesPath, AnodeType, cameraNumber, extension);

			logger.LogInformation("Sending images to the server ? {image.Exists} {name}", image.Exists, image.FullName);
			if (!image.Exists)
				return;

			StreamContent content = new(image.OpenRead());
			content.Headers.ContentType = new($"image/{extension}");
			content.Headers.ContentLength = image.Length;
			formData.Add(content, image.Name, image.Name);

		logger.LogInformation("Sending images to the server ? {formData}", formData.Any());

		if (!formData.Any())
			return;

		using HttpClient httpClient = new();
		httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));

		logger.LogInformation("Sending images to the server: http");
		HttpResponseMessage response
			= await httpClient.PostAsync($"{ITApisDict.ServerReceiveAddress}/apiServerReceive/images", formData);
		if (!response.IsSuccessStatusCode)
			throw new HttpRequestException("Could not send images to the server: " + response.ReasonPhrase);

		logger.LogInformation("Sending images to the server: {reponse}", response.ToString());
	}

	protected override Task InheritedBuild(IAnodeUOW anodeUOW)
	{
		if (Cam01Status == 1)
		{
			FileInfo image = GetImagePathFromRoot(StationCycleRID, Station.ID, ImagesPath, AnodeType, 1, Extension);
			HasFirstShoot = image.Exists;
		}

		if (Cam02Status == 1)
		{
			FileInfo image = GetImagePathFromRoot(StationCycleRID, Station.ID, ImagesPath, AnodeType, 2, Extension);
			HasSecondShoot = image.Exists;
		}

		HasError = HasError
			|| (Cam01Status.Equals(1) && Cam02Status.Equals(1))
			|| (Cam01Status.Equals(0) && Cam02Status.Equals(0));
		return Task.CompletedTask;
	}
}