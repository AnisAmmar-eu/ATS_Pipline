using Core.Entities.Vision.ToDos.Models.DB.ToSigns;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Models.DTO.ToSigns;

public partial class DTOToSign
{
	public DTOToSign()
	{
	}

	public DTOToSign(ToSign dtoSign) : base(dtoSign)
	{
		CycleID = dtoSign.CycleID;
		CycleRID = dtoSign.CycleRID;
		CameraID = dtoSign.CameraID;
		StationID = dtoSign.StationID;
		AnodeType = dtoSign.AnodeType;
		ShootingTS = dtoSign.ShootingTS;
	}

	public override ToSign ToModel()
	{
		return new(this);
	}
}