using Core.Entities.IOT.Data;

namespace Core.Shared.Data;

public class DBInitializer
{
	public DBInitializer()
	{
	}

	public static void Initialize(AnodeCTX anodeCTX)
	{
		IOTInitializer.Initialize(anodeCTX);
	}
}