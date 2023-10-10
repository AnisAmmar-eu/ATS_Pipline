using Core.Entities.Parameters.CameraParams.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;
using Stemmer.Cvb;
using Stemmer.Cvb.Driver;
using Stemmer.Cvb.GenApi;

namespace Core.Entities.Parameters.CameraParams.Models.DTO;

public partial class DTOCameraParam : DTOBaseEntity, IDTO<CameraParam, DTOCameraParam>
{
	public DTOCameraParam()
	{
	}

	public DTOCameraParam(CameraParam cameraParam)
	{
		ID = cameraParam.ID;
		TS = cameraParam.TS;
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

	public void GetCameraParams(Device device)
	{
		NodeMap deviceNodeMap = device.NodeMaps[NodeMapNames.Device];

		if (deviceNodeMap["TriggerMode"] is EnumerationNode triggerMode)
			TriggerMode = triggerMode.Value;

		if (deviceNodeMap["TriggerSource"] is EnumerationNode triggerSource)
			TriggerSource = triggerSource.Value;

		if (deviceNodeMap["TriggerActivation"] is EnumerationNode triggerActivation)
			TriggerActivation = triggerActivation.Value;

		if (deviceNodeMap["ExposureTime"] is FloatNode exposureTime)
			ExposureTime = exposureTime.Value;

		if (deviceNodeMap["PixelFormat"] is EnumerationNode pixelFormat)
			PixelFormat = pixelFormat.Value;

		if (deviceNodeMap["Width"] is IntegerNode width)
			Width = width.Value;

		if (deviceNodeMap["Height"] is IntegerNode height)
			Height = height.Value;

		if (deviceNodeMap["AcquisitionFrameRateEnable"] is EnumerationNode frameRateEnable)
			AcquisitionFrameRateEnable = frameRateEnable.Value;

		if (deviceNodeMap["Gain"] is FloatNode gain)
			Gain = gain.Value;

		if (deviceNodeMap["BlackLevel"] is FloatNode blackLevel)
			BlackLevel = blackLevel.Value;

		if (deviceNodeMap["Gamma"] is FloatNode gamma)
			Gamma = gamma.Value;

		if (deviceNodeMap["BalanceRatio"] is FloatNode balanceRatio)
			BalanceRatio = balanceRatio.Value;

		if (deviceNodeMap["ConvolutionMode"] is EnumerationNode { IsReadable: true } convolutionMode)
			ConvolutionMode = convolutionMode.Value;

		if (deviceNodeMap["AdaptiveNoiseSuppressionFactor"] is FloatNode noiseFactor)
			AdaptiveNoiseSuppressionFactor = noiseFactor.Value;

		if (deviceNodeMap["Sharpness"] is IntegerNode { IsReadable: true } sharpness)
			Sharpness = sharpness.Value;

		if (deviceNodeMap["AcquisitionFrameRate"] is FloatNode { IsReadable: true } frameRate)
			AcquisitionFrameRate = frameRate.Value;
	}

	public Device SetCameraParams(Device device)
	{
		NodeMap deviceNodeMap = device.NodeMaps[NodeMapNames.Device];

		if (deviceNodeMap["TriggerMode"] is EnumerationNode triggerMode)
			triggerMode.Value = TriggerMode;

		if (deviceNodeMap["TriggerSource"] is EnumerationNode triggerSource)
			triggerSource.Value = TriggerSource;

		if (deviceNodeMap["TriggerActivation"] is EnumerationNode triggerActivation)
			triggerActivation.Value = TriggerActivation;

		if (deviceNodeMap["ExposureTime"] is FloatNode exposureTime)
			exposureTime.Value = ExposureTime;

		if (deviceNodeMap["PixelFormat"] is EnumerationNode pixelFormat)
			pixelFormat.Value = PixelFormat;

		if (deviceNodeMap["Width"] is IntegerNode width)
			width.Value = Width;

		if (deviceNodeMap["Height"] is IntegerNode height)
			height.Value = Height;

		if (deviceNodeMap["AcquisitionFrameRateEnable"] is EnumerationNode frameRateEnable)
			frameRateEnable.Value = AcquisitionFrameRateEnable;

		if (deviceNodeMap["Gain"] is FloatNode gain)
			gain.Value = Gain;

		if (deviceNodeMap["BlackLevel"] is FloatNode blackLevel)
			blackLevel.Value = BlackLevel;

		if (deviceNodeMap["Gamma"] is FloatNode gamma)
			gamma.Value = Gamma;

		if (deviceNodeMap["BalanceRatio"] is FloatNode balanceRatio)
			balanceRatio.Value = BalanceRatio;

		if (deviceNodeMap["ConvolutionMode"] is EnumerationNode { IsWritable: true } convolutionMode)
			convolutionMode.Value = ConvolutionMode;

		if (deviceNodeMap["AdaptiveNoiseSuppressionFactor"] is FloatNode noiseFactor)
			noiseFactor.Value = AdaptiveNoiseSuppressionFactor;

		if (deviceNodeMap["Sharpness"] is IntegerNode { IsWritable: true } sharpness)
			sharpness.Value = Sharpness;

		if (deviceNodeMap["AcquisitionFrameRate"] is FloatNode { IsWritable: true } frameRate)
			frameRate.Value = AcquisitionFrameRate;

		return device;
	}
}