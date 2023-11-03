using TwinCAT;
using TwinCAT.Ads;

namespace Core.Shared.Models.TwinCat;

public static class TwinCatConnectionManager
{
	private static readonly AdsClient _tcClient = new();

	public static AdsClient Connect(int port)
	{
		if (_tcClient.IsConnected)
			return _tcClient;
		_tcClient.Connect(port);
		if (!_tcClient.IsConnected)
			throw new AdsException("Could not connect to the automaton");
		return _tcClient;
	}
}