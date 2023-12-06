using System.Diagnostics;
using Core.Entities.IOT.IOTTags.Services;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.Structs;
using Core.Shared.Dictionaries;
using Stemmer.Cvb;
using Stemmer.Cvb.Driver;
using TwinCAT.Ads;

namespace ApiCamera.Utils;

public static class CameraUtils
{
	private static IIOTTagService? _iotTagService;

	public static Task RunAcquisition(
		IIOTTagService iotTagService,
		Device device,
		string extension,
		string imagesDir,
		string testDir,
		CancellationToken cancel,
		string? imagesDir2 = null,
		string? testDir2 = null)
	{
		_iotTagService ??= iotTagService;

		Directory.CreateDirectory(imagesDir);

		if (imagesDir2 is not null)
			Directory.CreateDirectory(imagesDir2);

		Directory.CreateDirectory(testDir);
		if (testDir2 is not null)
			Directory.CreateDirectory(testDir2);

		device.Notify[NotifyDictionary.DeviceDisconnected].Event += Disconnect;
		device.Notify[NotifyDictionary.DeviceReconnect].Event += Reconnect;

		AdsClient tcClient = new();
		tcClient.Connect(851);
		if (!tcClient.IsConnected)
			throw new("Not connected");

		uint ridStructHandle = tcClient.CreateVariableHandle(ADSUtils.GlobalRIDForCamera);

		Stemmer.Cvb.Driver.Stream stream = device.Stream;
		if (!stream.IsRunning)
			stream.Start();

		int nbPictures = 0;
		int nbPicturesTest = 0;

		return Task.Run(
			() =>
			{
				while (!cancel.IsCancellationRequested)
				{
					try
					{
						if (!device.ConnectionState.HasFlag(ConnectionState.Connected))
						{
							Debug.WriteLine("connection lost");
							Thread.Sleep(1000);
							continue;
						}

						using StreamImage image = stream.Wait();
						if (IsTestModeOn())
						{
							// testDir2 != null means that we are in a S5 Cycle.
							string dir = (testDir2 is not null && nbPicturesTest % 2 == 1) ? testDir2 : testDir;
							FileInfo previousImage = new(dir + ShootingUtils.TestFilename);
							if (previousImage.Exists)
								previousImage.Delete();

							image.Save(dir + ShootingUtils.TestFilename, 1);
							++nbPicturesTest;
						}
						else
						{
							Console.WriteLine("Reading RID struct");
							RIDStruct ridStruct = tcClient.ReadAny<RIDStruct>(ridStructHandle);
							string rid = ridStruct.ToRID();
							string ts = DateTimeOffset.Now.ToString(AnodeFormat.RIDFormat);
							string filename = rid + "-" + ts + "." + extension;
							// imagesDir2 != null means that we are in a S5 Cycle.
							if (imagesDir2 is not null && nbPictures % 2 == 1)
								image.Save(imagesDir2 + filename, 1);
							else
								image.Save(imagesDir + filename, 1);

							++nbPictures;
						}
					}
					catch (Exception)
					{
						// Warning -> In prod, this condition is called before the disconnect function
						// If the program crash, put this inside a try/catch or create a global var
						// If the stream is always active, stop it
						if (device.Stream.IsRunning)
							stream.TryStop();
					}
				}

				if (device.Stream.IsRunning)
					stream.TryStop();

				return Task.CompletedTask;
			},
			cancel);
	}

	#region Generics functions

	private static bool IsTestModeOn()
	{
		//if (await iotTagService.IsTestModeOn())
		if (_iotTagService is null)
			throw new ArgumentException(nameof(_iotTagService));

		lock (_iotTagService)
            return _iotTagService.IsTestModeOnSync();
    }

	private static void Disconnect(object? sender, NotifyEventArgs e)
	{
		Console.WriteLine("disconnected");

		if (sender is null)
			return;

		Device? device = (Device?)sender.GetType().GetProperty("Parent")?.GetValue(sender);

		if (device is null)
			return;

		try
		{
			bool isOk = device.Stream.TryStop();
			if (!isOk)
				Debug.WriteLine("Could not stop the stream!");
		}
		catch (Exception ex)
		{
			Debug.WriteLine("Error: " + ex.Message);
		}
	}

	private static void Reconnect(object? sender, NotifyEventArgs e)
	{
		Console.WriteLine("reconnected");

		Device? device = (Device?)sender?.GetType().GetProperty("Parent")?.GetValue(sender);

		if (device is null)
			return;

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