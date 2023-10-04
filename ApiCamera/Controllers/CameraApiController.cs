using Microsoft.AspNetCore.Mvc;
using Stemmer.Cvb.Driver;
using Stemmer.Cvb;
using Stream = Stemmer.Cvb.Driver.Stream;
using System.Diagnostics;
using Stemmer.Cvb.Utilities;
using Stemmer.Cvb.GenApi;
using System.ComponentModel.DataAnnotations;
using TwinCAT.Ads;
using TwinCAT.TypeSystem;

namespace ApiCamera.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CameraApiController : ControllerBase
	{
		#region Generics functions

		private static void Disconnect(object? sender, NotifyEventArgs e)
		{
			Console.WriteLine("disconnected");


			if (sender == null)
				return;

			Device? device = (Device?)sender.GetType().GetProperty("Parent")?.GetValue(sender);

			if (device != null)
			{
				try
				{
					bool isOk = device.Stream.TryStop();
					if (!isOk)
					{
						Debug.WriteLine("Could not stop the stream!");
					}
				}
				catch (Exception ex)
				{
					Debug.WriteLine("Error: " + ex.Message);
				}
			}
		}

		private static void Reconnect(object? sender, NotifyEventArgs e)
		{
			Console.WriteLine("reconnected");

			if (sender == null)
				return;

			Device? device = (Device?)sender.GetType().GetProperty("Parent")?.GetValue(sender);

			if (device != null)
			{
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
		}

		#endregion

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
				{
					Result = "Driver Open Problem";
				}
				else
				{
					Result = "Driver Opened ";
				}
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}

			return Ok(Result);
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
				{
					try
					{
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
							Stopwatch stopWatch = new Stopwatch();
							stopWatch.Start();
							CancellationToken cancel = CancellationToken.None;

							AdsClient tcClient = new AdsClient();

							// Connection
							tcClient.Connect(851);
							if (!tcClient.IsConnected)
							{
								throw new Exception("Not connected");
							}

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
						if (device.Stream.IsRunning == true)
							stream.TryStop();
					}
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
				pixelFormat.Value = "RGB8";


			return device;
		}
		// Exposure Time
		// Gain
		// Black level
		// Gamma
		// Balance Ratio
		// Convolution Mode => AdaptiveNoiseSuppressionFactor
		//                  => Sharpness
		// Not configurable but displayed : AcquisitionFrameRate (w/ AcquisitionFrameRateEnable=false)
		//      displayed after ExposureTime
		// AutoBalance, Correlation etc... => off
		// PixelFormat = BayerRG8
		// Resolution = 2464 (H) x 2056 (V); 5.1 MP
		// Image format is uncompressed jpg format

		/*
		if (deviceNodeMap["Width"] is IntegerNode width)
		    width.Value = 2464;

		if (deviceNodeMap["Height"] is IntegerNode height)
		    height.Value = 2056;
		*/

		#endregion
	}
}