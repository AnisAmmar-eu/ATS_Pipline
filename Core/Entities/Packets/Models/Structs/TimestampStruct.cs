using System.Globalization;
using System.Runtime.InteropServices;

namespace Core.Entities.Packets.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Ansi)]
public struct TimestampStruct
{
	public int Year;
	public int Month;
	public int Day;
	public int Hour;
	public int Minutes;
	public int Sec;
	public int MS;

	public DateTimeOffset GetTimestamp()
	{
		try
		{
			return new(
				Year,
				Month,
				Day,
				Hour,
				Minutes,
				Sec,
				MS,
				new GregorianCalendar(GregorianCalendarTypes.Localized),
				TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow));
		}
		catch (ArgumentOutOfRangeException)
		{
			return DateTimeOffset.UnixEpoch;
		}
	}
}