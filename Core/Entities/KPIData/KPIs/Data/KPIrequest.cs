using Microsoft.AspNetCore.Mvc;

namespace Core.Entities.KPIData.KPIs.Data;

[BindProperties]
public class KPIrequest
{
	public List<string> OriginStations { get; set; } = [];
	public List<string> Anodes { get; set; } = [];
	public DateTimeOffset? Start { get; set; }
	public DateTimeOffset? End { get; set; }
}