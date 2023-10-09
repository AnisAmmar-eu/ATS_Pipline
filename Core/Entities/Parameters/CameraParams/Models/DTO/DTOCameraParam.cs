using Core.Entities.Parameters.CameraParams.Dictionaries;
using Core.Entities.Parameters.CameraParams.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Parameters.CameraParams.Models.DTO;

public partial class DTOCameraParam : DTOBaseEntity, IDTO<CameraParam, DTOCameraParam>
{
	public string TriggerMode { get; set; } = "On";
	public string TriggerSource { get; set; } = TriggerSources.Line3;
	public string TriggerActivation { get; set; } = TriggerActivations.AnyEdge;
	public double ExposureTime { get; set; } = 30000.0;
	public string PixelFormat { get; set; } = PixelFormats.BayerRG8;
	public int Width { get; set; } = 2464;
	public int Height { get; set; } = 2056;
	public string AcquisitionFrameRateEnable { get; set; } = "Off";
	public double Gain { get; set; } = 10;
	public double BlackLevel { get; set; } = 50;
	public double Gamma { get; set; } = 1;
	public double BalanceRatio { get; set; } = 2.35498;
	public string ConvolutionMode { get; set; } = "Off";
	public double AdaptiveNoiseSuppressionFactor { get; set; } = 1;
	public int Sharpness { get; set; } = 0;
	public double AcquisitionFrameRate { get; set; } = 23.9798;
}