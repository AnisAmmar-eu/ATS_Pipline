using System.Globalization;
using System.Linq.Expressions;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DTO;
using Core.Entities.Packets.Models.DTO.Shootings;
using Core.Entities.Packets.Models.Structs;
using Core.Entities.StationCycles.Models.DB;
using Core.Shared.Data;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Packets.Models.DB.Shootings;

public partial class Shooting : Packet, IBaseEntity<Shooting, DTOShooting>
{
	public Shooting()
	{
		Type = PacketType.Shooting;
	}

	public Shooting(DTOShooting dtoShooting) : base(dtoShooting)
	{
		Type = PacketType.Shooting;
		AnodeIDKey = dtoShooting.AnodeIDKey;
		GlobalStationStatus = dtoShooting.GlobalStationStatus;
		LedStatus = dtoShooting.LedStatus;
		ProcedurePerformance = dtoShooting.ProcedurePerformance;
		ShootingTS = dtoShooting.ShootingTS;
	}

	public Shooting(ShootingStruct adsStruct)
	{
		Type = PacketType.Shooting;
		// TODO
		// CycleStationRID = adsStruct.CycleStationRID;
		AnodeIDKey = (int)adsStruct.AnodeIDKey;
		GlobalStationStatus = adsStruct.GlobalStationStatus;
		ProcedurePerformance = (int)adsStruct.ProcedurePerformance;
		LedStatus = adsStruct.LedStatus;
		// TODO
		// ShootingTS = adsStruct.ShootingTS;
	}

	public override DTOShooting ToDTO()
	{
		return new DTOShooting(this);
	}

	protected override async Task<DTOPacket> InheritedBuild(IAnodeUOW anodeUOW, DTOPacket dtoPacket)
	{
		DirectoryInfo directory1 = new(ShootingFolders.Camera1);
		DirectoryInfo directory2 = new(ShootingFolders.Camera2);
		DirectoryInfo archive1 = new(ShootingFolders.Archive1);
		DirectoryInfo archive2 = new(ShootingFolders.Archive2);
		if (!directory1.Exists || !directory2.Exists || !archive1.Exists || !archive2.Exists)
			throw new IOException("One or more ShootingFolders do not exist");
		// First and Third hole because second hole isn't taken into account.
		FileInfo? firstHole = null;
		FileInfo? thirdHole = null;
		DateTimeOffset? tsFirstImage = null;
		string rid = string.Empty;
		while (firstHole == null || thirdHole == null)
		{
			if (tsFirstImage != null && DateTimeOffset.Now - tsFirstImage > TimeSpan.FromSeconds(30))
				break;
			if (firstHole == null)
			{
				firstHole = GetImageInDirectory(directory1, rid);
				if (firstHole != null)
				{
					if (thirdHole == null)
						rid = ExtractRIDFromName(firstHole.Name);
					DateTimeOffset tsHoleImage =
						DateTimeOffset.ParseExact(ExtractTSFromName(firstHole.Name, rid), "yyyyMMddHHmmssfff",
							CultureInfo.InvariantCulture.DateTimeFormat);
					tsFirstImage = tsFirstImage == null || tsHoleImage < tsFirstImage ? tsHoleImage : tsFirstImage;
				}
			}

			if (thirdHole == null)
			{
				thirdHole = GetImageInDirectory(directory2, rid);
				if (thirdHole != null)
				{
					if (firstHole == null)
						rid = ExtractRIDFromName(thirdHole.Name);
					DateTimeOffset tsHoleImage =
						DateTimeOffset.ParseExact(ExtractTSFromName(thirdHole.Name, rid), "yyyyMMddHHmmssfff", null);
					tsFirstImage = tsFirstImage == null || tsHoleImage < tsFirstImage ? tsHoleImage : tsFirstImage;
				}
			}
		}

		StationCycle = await anodeUOW.StationCycle.GetBy(
			new Expression<Func<StationCycle, bool>>[]
			{
				stationCycle => stationCycle.RID == rid
			}, withTracking: false);
		StationCycle.ShootingPacket = this;
		StationCycle.ShootingID = ID;
		// ?. => If firstHole not null then...
		while (IsFileLocked(firstHole) || IsFileLocked(thirdHole));
		firstHole?.MoveTo(ShootingFolders.Archive1 + firstHole.Name);
		thirdHole?.MoveTo(ShootingFolders.Archive2 + thirdHole.Name);
		Status = PacketStatus.Completed;
		ShootingTS = (DateTimeOffset)tsFirstImage!;
		HasError = firstHole == null || thirdHole == null;
		StationCycleRID = rid!;
		StationCycle.ShootingStatus = Status;
		anodeUOW.StationCycle.Update(StationCycle);
		StationCycle = StationCycle;
		return ToDTO();
	}

	private FileInfo? GetImageInDirectory(DirectoryInfo directory, string rid)
	{
		List<FileInfo> images = directory.EnumerateFiles().ToList()
			.FindAll(fileInfo => rid == string.Empty || ExtractRIDFromName(fileInfo.Name) == rid);
		images.Sort((x, y) => DateTime.Compare(x.CreationTime, y.CreationTime));
		if (images.Count == 0)
			return null;
		return images[0];
	}

	private bool IsFileLocked(FileInfo? file)
	{
		if (file == null) return false;
		try
		{
			using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
			{
				stream.Close();
			}
		}
		catch (IOException)
		{
			return true;
		}

		return false;
	}

	private string ExtractRIDFromName(string fileName)
	{
		if (!string.IsNullOrWhiteSpace(fileName))
		{
			int charLocation = fileName.IndexOf("-", StringComparison.Ordinal);
			if (charLocation > 0) return fileName.Substring(0, charLocation);
		}

		return string.Empty;
	}

	private string ExtractTSFromName(string fileName, string? rid)
	{
		if (rid != null && !string.IsNullOrWhiteSpace(fileName))
		{
			int charLocation = fileName.IndexOf(".", StringComparison.Ordinal);
			charLocation = (charLocation == 0 ? fileName.Length : charLocation) - rid.Length - 1;
			if (charLocation > rid.Length + 1) return fileName.Substring(rid.Length + 1, charLocation);
		}

		return string.Empty;
	}
}