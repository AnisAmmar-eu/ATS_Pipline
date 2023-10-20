using System.Diagnostics;
using System.Globalization;
using Core.Entities.Packets.Models.Structs;
using Core.Shared.Dictionaries;
using Stemmer.Cvb;
using Stemmer.Cvb.Driver;
using Stemmer.Cvb.GenApi;
using TwinCAT.Ads;
using Stream = Stemmer.Cvb.Driver.Stream;

namespace ApiCamera.Utils;

public static class CameraUtils
{
	public static void RunAcquisition(Device device, string extension, string imagesDir,
		string? imagesDir2 = null)
	{
		Directory.CreateDirectory(imagesDir);
		device.Notify[NotifyDictionary.DeviceDisconnected].Event += Disconnect;
		device.Notify[NotifyDictionary.DeviceReconnect].Event += Reconnect;

		CancellationToken cancel = CancellationToken.None;
		AdsClient tcClient = new();
		tcClient.Connect(851);
		if (!tcClient.IsConnected) throw new Exception("Not connected");
		uint oldEntryHandle = tcClient.CreateVariableHandle(ADSUtils.DetectionToRead);

		Stream stream = device.Stream;
		if (!stream.IsRunning)
			stream.Start();
		int nbPictures = 0;
		while (true)
			try
			{
				// TODO Remove when testing IOTTags
				// If the stream is stopped because parameters are being modified, waits for it to come back.
				if (!stream.IsRunning)
				{
					Thread.Sleep(100);
					continue;
				}

				if (device.ConnectionState.HasFlag(ConnectionState.Connected) != true)
				{
					Debug.WriteLine("connection lost");
					Thread.Sleep(1000);
					continue;
				}

				using (StreamImage image = stream.Wait())
				{
					DetectionStruct detection = tcClient.ReadAny<DetectionStruct>(oldEntryHandle);
					string rid = detection.StationCycleRID.ToRID();
					string ts = DateTimeOffset.Now.ToString("yyyyMMddHHmmssfff");
					string filename = rid + "-" + ts + "." + extension;
					// imagesDir2 != null means that we are in a S5 Cycle.
					if (imagesDir2 != null && nbPictures % 2 == 1)
						image.Save(imagesDir2 + filename, 1);
					else image.Save(imagesDir + filename, 1);
					++nbPictures;
				}
			}
			catch (Exception ex)
			{
				// Warning -> In prod, this condition is called before the disconnect function
				// If the program crash, put this inside a try/catch or create a global var
				// If the stream is always active, stop it
				if (device.Stream.IsRunning)
					stream.TryStop();
			}
	}

	public static void SetParameters(Device device, Dictionary<string, string> parameters)
	{
		NodeMap nodeMap = device.NodeMaps[NodeMapNames.Device];
		foreach ((string? path, string? newValue) in parameters)
			if (nodeMap[path].IsWritable)
				switch (nodeMap[path])
				{
					case IntegerNode { IsWritable: true } integerNode:
						integerNode.Value = int.Parse(newValue);
						break;
					case FloatNode { IsWritable: true } floatNode:
						floatNode.Value = double.Parse(newValue, CultureInfo.InvariantCulture);
						break;
					case EnumerationNode { IsWritable: true } enumerationNode:
						enumerationNode.Value = newValue;
						break;
					default:
						throw new InvalidOperationException("Camera tag with path " + path +
						                                    " has a path towards unsupported data type.");
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