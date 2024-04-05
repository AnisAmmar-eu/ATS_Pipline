using Core.Entities.Vision.ToDos.Models.DB;
using Core.Shared.Dictionaries;

namespace Core.Entities.Vision.Dictionaries;

public enum InstanceMatchID
{
	S3 = 0,
	S4 = 1,
	S3S4 = 2,
	S5 = 3,
	S5_C = 4,
}

public enum DataSetID
{
	/// <summary>DataSetID</summary>
	/// <remarks>Cam1 DX 0</remarks>
	/// <remarks>Cam1 D20 1</remarks>
	/// <remarks>Cam2 DX 2</remarks>
	/// <remarks>Cam2 D20 3</remarks>
	/// <remarks>To retrieve in config file</remarks>
	Cam1DX = 0,
	Cam1D20 = 1,
	Cam2DX = 2,
	Cam2D20 = 3,
}
public static class DataSets
{
	public static DataSetID TodoToDataSetID(ToDos.Models.DB.ToDo todo)
	{
		return todo switch
		{
			{ CameraID: 1, AnodeType: AnodeTypeDict.DX } => DataSetID.Cam1DX,
			{ CameraID: 1, AnodeType: AnodeTypeDict.D20 } => DataSetID.Cam1D20,
			{ CameraID: 2, AnodeType: AnodeTypeDict.DX } => DataSetID.Cam2DX,
			{ CameraID: 2, AnodeType: AnodeTypeDict.D20 } => DataSetID.Cam2D20,
			_ => throw new NotImplementedException(),
		};
	}
}