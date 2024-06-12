using System.Globalization;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Core.Entities.IOT.Dictionaries;
using Core.Entities.Packets.Models.DTO.Shootings;
using Core.Shared.Dictionaries;
using Core.Shared.UnitOfWork.Interfaces;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Core.Entities.Packets.Models.DB.Shootings;

public partial class Shooting
{
	public Shooting()
	{
	}

	public override DTOShooting ToDTO() => this.Adapt<DTOShooting>();

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
				= $@"S{stationID:00}\T{anodeType}\Y{ridTS.Year.ToString()}\M{ridTS.Month:00}\D{ridTS.Day:00}\C{camera:00}\";
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
			= $"S{groups["stationID"].Value}\\T{groups["anodeType"]}\\Y{date.Year.ToString()}"
				+ $"\\M{date.Month:00}\\D{date.Day:00}\\C{groups["camera"]}\\";

		return new($@"{root}\{path}\{filename}");
	}

	[GeneratedRegex("S(?<stationID>[0-9]{2})T(?<anodeType>.{2})C(?<camera>[0-9]{2})T(?<TS>.*)")]
	private static partial Regex ServerFilenameRegex();

	public static string GetCycleRIDFromFilename(string filename)
	{
		Regex regex = ServerFilenameRegex();
		GroupCollection groups = regex.Match(filename).Groups;
		return $"{int.Parse(groups["stationID"].Value).ToString()}_{groups["TS"].Value}";
	}

	public async Task SendImages(string imagesPath, string thumbnailsPath, string extension, ILogger logger)
	{
		foreach (string path in (List<string>)([imagesPath, thumbnailsPath]))
		{
			int cameraNumber = (Cam01Status == 1) ? 1 : 2;
			MultipartFormDataContent formData = [];
			formData.Headers.ContentType!.MediaType = "multipart/form-data";

			int stationID = int.Parse(StationCycleRID[0].ToString());
			FileInfo image
				= GetImagePathFromRoot(StationCycleRID, stationID, path, AnodeType, cameraNumber, extension);

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
			bool isImage = path == imagesPath;
			HttpResponseMessage response
				= await httpClient.PostAsync(
					$"{ITApisDict.ServerReceiveAddress}/apiServerReceive/images/{isImage.ToString()}",
					formData);
			if (!response.IsSuccessStatusCode)
				throw new HttpRequestException("Could not send images to the server: " + response.ReasonPhrase);

			logger.LogInformation("Sending images to the server: {reponse}", response.ToString());
		}
	}

	protected override Task InheritedBuild(IAnodeUOW anodeUOW)
	{
		HasError = HasError
			|| (Cam01Status == 0 && Cam02Status == 0)
			|| (Cam01Status == 1 && Cam02Status == 1);

		return Task.CompletedTask;
	}
}