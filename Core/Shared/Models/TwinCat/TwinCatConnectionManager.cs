using TwinCAT;
using TwinCAT.Ads;

namespace Core.Shared.Models.TwinCat;

public static class TwinCatConnectionManager
{
	private static readonly AdsClient TcClient = new();

	public static async Task<AdsClient> Connect(int port, CancellationToken cancel)
	{
		if (TcClient.IsConnected)
			return TcClient;

		return await Task.Run(
			() =>
			{
				while (!cancel.IsCancellationRequested)
				{
					try
					{
						TcClient.Connect(port);
						if (!TcClient.IsConnected)
							throw new AdsException("Could not connect to the automaton");

						return Task.FromResult(TcClient);
					}
					catch
					{
						// Ignored
					}
				}

				throw new IOException("Could not connect to the automaton");
			},
			cancel);
	}
}