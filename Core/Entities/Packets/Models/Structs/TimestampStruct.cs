using System.Globalization;
using System.Runtime.InteropServices;

namespace Core.Entities.Packets.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Ansi)]
public struct TimestampStruct
{
	public ushort Year;
	public ushort Month;
	public ushort Day;
	public ushort Hour;
	public ushort Minutes;
	public ushort Sec;
	public ushort MS;

	public DateTimeOffset GetTimestamp()
	{
		return new DateTimeOffset(Year, Month, Day, Hour, Minutes, Sec, MS,
			new GregorianCalendar(GregorianCalendarTypes.Localized),
			TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow));
	}
}