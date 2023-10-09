using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Core.Entities.Parameters.CameraParams.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Stemmer.Cvb;
using Stemmer.Cvb.Driver;
using Stemmer.Cvb.GenApi;
using TwinCAT.Ads;
using TwinCAT.TypeSystem;
using Stream = Stemmer.Cvb.Driver.Stream;

namespace ApiCamera.Controllers;

[ApiController]
[Route("[controller]")]
public class CameraApiController : ControllerBase
{
	#region Get/Set Device Info

	[HttpGet("/GetDeviceInfo")]
	public IActionResult GetDeviceInfo()
	{
		string driverString = Environment.ExpandEnvironmentVariables("%CVB%") + @"Drivers\\GenICam.vin";
		string Result = "";
		try
		{
			Device device = DeviceFactory.Open(driverString);

			NodeMap deviceNodeMap = device.NodeMaps[NodeMapNames.Device];

			Debug.WriteLine("DeviceTemperature 1: " + deviceNodeMap["DeviceTemperature"] + "°");

			Debug.WriteLine("Vendor " + deviceNodeMap["DeviceVendorName"]);

			Debug.WriteLine("DeviceTLVersionMinor " + deviceNodeMap["DeviceTLVersionMinor"]);

			Debug.WriteLine("DeviceFamilyName " + deviceNodeMap["DeviceFamilyName"]);

			if (deviceNodeMap["ExposureTime"] is FloatNode exposure)
				exposure.Value = 40000.5;

			Debug.WriteLine("DeviceTemperature 2: " + deviceNodeMap["DeviceTemperature"] + "°");


			if (device == null)
				Result = "Driver Open Problem";
			else
				Result = "Driver Opened ";
		}
		catch (Exception e)
		{
			return BadRequest(e.Message);
		}

		return Ok(Result);
	}

	[HttpGet("parameters")]
	public IActionResult GetDeviceParameters()
	{
		string driverString = Environment.ExpandEnvironmentVariables("%CVB%") + @"Drivers\\GenICam.vin";
		DTOCameraParam dtoCameraParam = new();
		try
		{
			Device device = DeviceFactory.Open(driverString);
			dtoCameraParam.GetCameraParams(device);
		}
		catch (Exception e)
		{
			return BadRequest(e.Message);
		}

		return Ok(dtoCameraParam);
	}

	[HttpPost("SetDeviceParameters")]
	public IActionResult SetDeviceParameters([FromBody] [Required] DTOCameraParam dtoCameraParam)
	{
		string driverString = Environment.ExpandEnvironmentVariables("%CVB%") + @"Drivers\\GenICam.vin";
		try
		{
			Device device = DeviceFactory.Open(driverString);
			if (device.Stream.IsRunning)
				device.Stream.Stop();
			dtoCameraParam.SetCameraParams(device);
			if (!device.Stream.IsRunning)
				device.Stream.Start();
		}
		catch (Exception e)
		{
			return BadRequest(e.Message);
		}

		return Ok();
	}

	#endregion

	#region Generics functions

	private static void Disconnect(object? sender, NotifyEventArgs e)
	{
		Console.WriteLine("disconnected");


		if (sender == null)
			return;

		Device? device = (Device?)sender.GetType().GetProperty("Parent")?.GetValue(sender);

		if (device != null)
			try
			{
				bool isOk = device.Stream.TryStop();
				if (!isOk) Debug.WriteLine("Could not stop the stream!");
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Error: " + ex.Message);
			}
	}

	private static void Reconnect(object? sender, NotifyEventArgs e)
	{
		Console.WriteLine("reconnected");

		if (sender == null)
			return;

		Device? device = (Device?)sender.GetType().GetProperty("Parent")?.GetValue(sender);

		if (device != null)
			try
			{
				device = SetParams(device);
				device.Stream.Start();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Error: " + ex.Message);
			}
	}

	#endregion

	#region Acquisition

	[HttpGet("/Acquisition")]
	public async Task<string> AcquisitionAsync([Required] string extention)
	{
		string driverString = Environment.ExpandEnvironmentVariables("%CVB%") + @"Drivers\\GenICam.vin";

		string Result = "";
		try
		{
			// Create an instance of the camera
			Device? device = DeviceFactory.Open(driverString);

			Result = "Driver Opened";

			// Set device params
			device = SetParams(device);

			// Set events (disconnect + reconnect)
			device.Notify[NotifyDictionary.DeviceDisconnected].Event += Disconnect;
			device.Notify[NotifyDictionary.DeviceReconnect].Event += Reconnect;

			// Get the current stream
			Stream stream = device.Stream;

			// Start the stream
			stream.Start();

			while (true)
				try
				{
					// If the stream is stopped because parameters are being modified, waits for it to come back.
					if (!stream.IsRunning)
					{
						Thread.Sleep(100);
						continue;
					}

					// Check if the camera is connected
					if (device.ConnectionState.HasFlag(ConnectionState.Connected) != true)
					{
						Debug.WriteLine("connection lost");
						Thread.Sleep(1000);
						continue;
					}

					DateTime now = DateTime.Now;
					string imageName = ((DateTimeOffset)now).ToUnixTimeSeconds().ToString();

					// Wait for a new photo to be taken
					using (StreamImage image = stream.Wait())
					{
						Stopwatch stopWatch = new();
						stopWatch.Start();
						CancellationToken cancel = CancellationToken.None;

						AdsClient tcClient = new();

						// Connection
						tcClient.Connect(851);
						if (!tcClient.IsConnected) throw new Exception("Not connected");

						// Use variable nCounter 
						uint varHandle = tcClient.CreateVariableHandle("MAIN.RID");

						// Read 20 - 30 ms runtime
						ResultAnyValue resultRead = await tcClient.ReadAnyStringAsync(varHandle, 80,
							StringMarshaler.DefaultEncoding, cancel);
						string RID = resultRead.Value.ToString();
						string Ts = DateTime.Now.ToString("yyyyMMddHHmmfff");

						string filename = RID + "-" + Ts + "." + extention;
						// Save the photo
						image.Save("anodesImages\\" + filename, 1);
						stopWatch.Stop();
						Debug.WriteLine($"{stopWatch.Elapsed.TotalMilliseconds}");
					}

					Debug.WriteLine("isRunning = " + device.Stream.IsRunning);
				}
				catch (Exception ex)
				{
					Debug.WriteLine("Exception: " + ex.Message);

					// Warning -> In prod, this condition is called before the disconnect function
					// If the program crash, put this inside a try/catch or create a global var
					// If the stream is always active, stop it
					if (device.Stream.IsRunning)
						stream.TryStop();
				}

			// Stop the stream
			stream.Stop();
		}
		catch (Exception e)
		{
			Result = e.Message;
			Debug.WriteLine("Catch End");
		}

		return Result;
	}


	private static Device SetParams(Device device)
	{
		NodeMap deviceNodeMap = device.NodeMaps[NodeMapNames.Device];

		if (deviceNodeMap["TriggerMode"] is EnumerationNode triggerMode)
			triggerMode.Value = "On";

		if (deviceNodeMap["TriggerSource"] is EnumerationNode triggerSource)
			triggerSource.Value = "Line3";

		if (deviceNodeMap["TriggerActivation"] is EnumerationNode triggerActivation)
			triggerActivation.Value = "AnyEdge";

		if (deviceNodeMap["ExposureTime"] is FloatNode exposureTime)
			exposureTime.Value = 30000.0;

		if (deviceNodeMap["PixelFormat"] is EnumerationNode pixelFormat)
			pixelFormat.Value = "BayerRG8";

		if (deviceNodeMap["Width"] is IntegerNode width)
			width.Value = 2464;

		if (deviceNodeMap["Height"] is IntegerNode height)
			height.Value = 2056;

		if (deviceNodeMap["AcquisitionFrameRateEnable"] is EnumerationNode frameRateEnable)
			frameRateEnable.Value = "Off";

		if (deviceNodeMap["Gain"] is FloatNode gain)
			gain.Value = 10;

		if (deviceNodeMap["BlackLevel"] is FloatNode blackLevel)
			blackLevel.Value = 50;

		if (deviceNodeMap["Gamma"] is FloatNode gamma)
			gamma.Value = 1;

		if (deviceNodeMap["BalanceRatio"] is FloatNode balanceRatio)
			balanceRatio.Value = 2.35498;

		if (deviceNodeMap["ConvolutionMode"] is EnumerationNode convolutionMode && convolutionMode.IsWritable)
			convolutionMode.Value = "Off";

		if (deviceNodeMap["AdaptiveNoiseSuppressionFactor"] is FloatNode noiseFactor)
			noiseFactor.Value = 1;

		if (deviceNodeMap["Sharpness"] is IntegerNode sharpness && sharpness.IsWritable)
			sharpness.Value = 0;

		if (deviceNodeMap["AcquisitionFrameRate"] is FloatNode frameRate && frameRate.IsWritable)
			frameRate.Value = 23.9798;
		return device;
	}
	// Exposure Time => AcquisitionControl

	// Gain => AnalogControl => GainSelector

	// Black level => AnalogControl => BlackLevelSelector

	// Gamma => MultipleRegionControl (SubRegionSelector) & Analog Control

	// Balance Ratio => MultipleRegionControl (SubRegionSelector) & AnalogControl (BalanceRatioSelector Red || Blue)

	// Convolution Mode => ImageProcessingControl

	// AdaptiveNoiseSuppressionFactor => ImageProcessingControl

	// Sharpness => ImageProcessingControl

	// Not configurable but displayed : AcquisitionFrameRate (w/ AcquisitionFrameRateEnable=false)
	//      displayed after ExposureTime
	// AcquisitionFrameRate => AcquisitionControl

	// AutoBalance, Correlation etc... => off

	// PixelFormat = BayerRG8

	// Resolution = 2464 (H) x 2056 (V); 5.1 MP

	// Image format is uncompressed jpg format

	/*
	*/

	#endregion
}