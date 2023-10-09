using Core.Entities.Parameters.CameraParams.Dictionaries;
using Core.Entities.Parameters.CameraParams.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Parameters.CameraParams.Models.DB;

public partial class CameraParam : BaseEntity, IBaseEntity<CameraParam, DTOCameraParam>
{
	public bool TriggerMode { get; set; } = true;
	public string TriggerSource { get; set; } = TriggerSources.Line3;
	public string TriggerActivation { get; set; } = TriggerActivations.AnyEdge;
	public double ExposureTime { get; set; } = 30000.0;
	public string PixelFormat { get; set; } = PixelFormats.BayerRG8;
	public int Width { get; set; } = 2464;
	public int Height { get; set; } = 2056;
	public bool AcquisitionFrameRateEnable { get; set; } = false;
	public double Gain { get; set; } = 10;
	public double BlackLevel { get; set; } = 50;
	public double Gamma { get; set; } = 1;
	public double BalanceRatio { get; set; } = 2.35498;
	public bool ConvolutionMode { get; set; } = false;
	public double AdaptiveNoiseSuppressionFactor { get; set; } = 1;
	public int Sharpness { get; set; } = 0;
	public double AcquisitionFrameRate { get; set; } = 23.9798;
}