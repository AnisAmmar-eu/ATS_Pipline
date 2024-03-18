using Core.Entities.Vision.ToDo.Models.DTO.ToSigns;

namespace Core.Entities.Vision.ToDo.Models.DB.ToSigns;

public partial class ToSign
{
	public ToSign()
	{
	}

	public ToSign(DTOToSign dtoSign) : base(dtoSign)
	{
		CycleID = dtoSign.CycleID;
		CycleRID = dtoSign.CycleRID;
		CameraID = dtoSign.CameraID;
		AnodeType = dtoSign.AnodeType;
		ShootingTS = dtoSign.ShootingTS;
	}

	public override DTOToSign ToDTO()
	{
		return new(this);
	}
}