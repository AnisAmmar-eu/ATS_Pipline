using TwinCAT;
using TwinCAT.Ads;

namespace Core.Shared.Models.TwinCat;

public static class TwinCatConnectionManager
{
	private static readonly AdsClient TcClient = new();

	public static AdsClient Connect(int port)
	{
		if (TcClient.IsConnected)
			return TcClient;
		TcClient.Connect(port);
		if (!TcClient.IsConnected)
			throw new AdsException("Could not connect to the automaton");
		return TcClient;
	}
}