namespace Core.Entities.KPI.KPIEntries.Dictionaries;

public class KPIPeriod
{
	public const string Day = "DAY";
	public const string Week = "WEEK";
	public const string Month = "MONTH";
	public const string Year = "YEAR";

	#region Methods
	public static DateTimeOffset GetStartRange(string period, DateTimeOffset now)
	{
		return period switch
		{
			Day => now - TimeSpan.FromHours(24),
			Week => now - TimeSpan.FromDays(7),
			Month => now - TimeSpan.FromDays(GetDaysInMonth(now.Month, now.Year)),
			Year => now - TimeSpan.FromDays(IsLeapYear(now.Year) ? 366 : 365),
			_ => throw new InvalidOperationException(period + " is not a valid period")
		};
	}

	private static int GetDaysInMonth(int month, int year)
	{
		return month == 2
			? IsLeapYear(year) ? 29 : 28
			: 31 - (month - 1) % 7 % 2;
	}

	private static bool IsLeapYear(int year)
	{
		return year % 4 == 0 && (year % 100 == 4 && year % 400 != 0);
	}
	#endregion
}