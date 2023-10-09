using Core.Entities.Parameters.CameraParams.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;
using Stemmer.Cvb;
using Stemmer.Cvb.Driver;
using Stemmer.Cvb.GenApi;

namespace Core.Entities.Parameters.CameraParams.Models.DTO;

public partial class DTOCameraParam : DTOBaseEntity, IDTO<CameraParam, DTOCameraParam>
{
	public DTOCameraParam(CameraParam cameraParam)
	{
		TriggerMode = cameraParam.TriggerMode ? "On" : "Off";
		TriggerSource = cameraParam.TriggerSource;
		TriggerActivation = cameraParam.TriggerActivation;
		ExposureTime = cameraParam.ExposureTime;
		PixelFormat = cameraParam.PixelFormat;
		Width = cameraParam.Width;
		Height = cameraParam.Height;
		AcquisitionFrameRateEnable = cameraParam.AcquisitionFrameRateEnable ? "On" : "Off";
		Gain = cameraParam.Gain;
		BlackLevel = cameraParam.BlackLevel;
		Gamma = cameraParam.Gamma;
		BalanceRatio = cameraParam.BalanceRatio;
		ConvolutionMode = cameraParam.ConvolutionMode ? "On" : "Off";
		AdaptiveNoiseSuppressionFactor = cameraParam.AdaptiveNoiseSuppressionFactor;
		Sharpness = cameraParam.Sharpness;
		AcquisitionFrameRate = cameraParam.AcquisitionFrameRate;
	}

	public Device SetCameraParams(Device device)
	{
		NodeMap deviceNodeMap = device.NodeMaps[NodeMapNames.Device];

		// During acq
		if (deviceNodeMap["TriggerMode"] is EnumerationNode { IsWritable: true } triggerMode)
			triggerMode.Value = TriggerMode;

		if (deviceNodeMap["TriggerSource"] is EnumerationNode triggerSource)
			triggerSource.Value = TriggerSource;

		if (deviceNodeMap["TriggerActivation"] is EnumerationNode triggerActivation)
			triggerActivation.Value = TriggerActivation;

		if (deviceNodeMap["ExposureTime"] is FloatNode exposureTime)
			exposureTime.Value = ExposureTime;

		// During acq
		if (deviceNodeMap["PixelFormat"] is EnumerationNode { IsWritable: true } pixelFormat)
			pixelFormat.Value = PixelFormat;

		// During acq
		if (deviceNodeMap["Width"] is IntegerNode { IsWritable: true } width)
			width.Value = Width;

		// During acq
		if (deviceNodeMap["Height"] is IntegerNode { IsWritable: true } height)
			height.Value = Height;

		// During acq
		if (deviceNodeMap["AcquisitionFrameRateEnable"] is EnumerationNode { IsWritable: true } frameRateEnable)
			frameRateEnable.Value = "Off";

		if (deviceNodeMap["Gain"] is FloatNode gain)
			gain.Value = Gain;

		// During acq
		if (deviceNodeMap["BlackLevel"] is FloatNode { IsWritable: true } blackLevel)
			blackLevel.Value = BlackLevel;

		if (deviceNodeMap["Gamma"] is FloatNode gamma)
			gamma.Value = Gamma;

		if (deviceNodeMap["BalanceRatio"] is FloatNode balanceRatio)
			balanceRatio.Value = BalanceRatio;

		if (deviceNodeMap["ConvolutionMode"] is EnumerationNode { IsWritable: true } convolutionMode)
			convolutionMode.Value = "Off";

		// During acq
		if (deviceNodeMap["AdaptiveNoiseSuppressionFactor"] is FloatNode { IsWritable: true } noiseFactor)
			noiseFactor.Value = AdaptiveNoiseSuppressionFactor;

		if (deviceNodeMap["Sharpness"] is IntegerNode { IsWritable: true } sharpness)
			sharpness.Value = Sharpness;

		if (deviceNodeMap["AcquisitionFrameRate"] is FloatNode { IsWritable: true } frameRate)
			frameRate.Value = AcquisitionFrameRate;

		return device;
	}
}