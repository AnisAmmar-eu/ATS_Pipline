using System.Diagnostics;
using Core.Entities.Parameters.CameraParams.Models.DTO;
using Stemmer.Cvb;
using Stemmer.Cvb.Driver;
using TwinCAT.Ads;
using TwinCAT.TypeSystem;
using Stream = Stemmer.Cvb.Driver.Stream;

namespace ApiCamera.Utils;

public static class CameraMethod
{
	public static async Task RunAcquisitionAsync(Device device, DTOCameraParam dtoCameraParam, string extension)
	{
		// Device params are managed by ApiIOT.

		// Set events (disconnect + reconnect)
		device.Notify[NotifyDictionary.DeviceDisconnected].Event += Disconnect;
		device.Notify[NotifyDictionary.DeviceReconnect].Event += Reconnect;

		// Get the current stream
		Stream stream = device.Stream;

		// Start the stream
		if (!stream.IsRunning)
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

				DateTimeOffset now = DateTimeOffset.Now;

				// Wait for a new photo to be taken
				using (StreamImage image = stream.Wait())
				{
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
					string Ts = DateTimeOffset.Now.ToString("yyyyMMddHHmmssfff");

					string filename = RID + "-" + Ts + "." + extension;
					// Save the photo
					image.Save("anodesImages\\" + filename, 1);
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
	}

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
				device.Stream.Start();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Error: " + ex.Message);
			}
	}

	#endregion
}