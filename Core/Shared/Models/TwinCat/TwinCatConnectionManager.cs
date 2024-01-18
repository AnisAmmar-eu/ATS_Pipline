using Core.Shared.Dictionaries;
using TwinCAT;
using TwinCAT.Ads;

namespace Core.Shared.Models.TwinCat;

public static class TwinCatConnectionManager
{
	private static readonly AdsClient TcClient = new();

	private const string ConnectionPath = ADSUtils.ConnectionPath;

	public static async Task<AdsClient> Connect(int port, CancellationToken cancel)
	{
		try
		{
			// Tricky but if it crashes, we're not connected either.
			uint handle = TcClient.CreateVariableHandle(ConnectionPath);
			TcClient.ReadAny<bool>(handle);
		}
		catch(Exception ex)
		{
			return await Task.Run(
				() =>
				{
					while (!cancel.IsCancellationRequested)
					{
						try
						{
							TcClient.Connect(port);
							if (!TcClient.IsConnected)
								throw new AdsException("Could not connect to the automaton {error}", ex);
							// Tricky but if it crashes, we're not connected either.
							uint handle = TcClient.CreateVariableHandle(ConnectionPath);
							TcClient.ReadAny<bool>(handle);

							return Task.FromResult(TcClient);
						}
						catch
						{
							// Ignored
						}
					}

					throw new IOException($"Could not connect to the automaton with {ConnectionPath}: {ex}");
				},
				cancel);
		}

		return TcClient;
	}
}