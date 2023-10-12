using System.Globalization;

namespace Core.Entities.Packets.Models.Structs;

public struct TimestampStruct
{
	public uint Year { get; set; }
	public uint Month { get; set; }
	public uint Day { get; set; }
	public uint Hour { get; set; }
	public uint Minutes { get; set; }
	public uint Sec { get; set; }
	public uint MS { get; set; }

	public DateTimeOffset GetTimestamp()
	{
		return new DateTimeOffset((int)Year, (int)Month, (int)Day, (int)Hour, (int)Minutes, (int)Sec,
			(int)MS, new GregorianCalendar(GregorianCalendarTypes.Localized), TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow));
	}
}