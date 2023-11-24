using Microsoft.AspNetCore.Authorization;

namespace Core.Shared.SignalR.StationCycleHub;

[Authorize]
public class StationCycleHub : BaseHub<IStationCycleHub>;