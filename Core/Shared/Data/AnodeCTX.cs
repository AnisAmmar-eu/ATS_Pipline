using Core.Entities.Alarms.AlarmsC.Models.DB;
using Core.Entities.Alarms.AlarmsCycle.Models.DB;
using Core.Entities.Alarms.AlarmsLog.Models.DB;
using Core.Entities.Alarms.AlarmsRT.Models.DB;
using Core.Entities.Anodes.Models.DB;
using Core.Entities.Anodes.Models.DB.AnodesD20;
using Core.Entities.Anodes.Models.DB.AnodesDX;
using Core.Entities.BenchmarkTests.Models.DB;
using Core.Entities.BenchmarkTests.Models.DB.CameraTests;
using Core.Entities.BI.BITemperatures.Models.DB;
using Core.Entities.DebugsModes.Models.DB;
using Core.Entities.IOT.IOTDevices.Models.DB;
using Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices.Matchs;
using Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices.Signs;
using Core.Entities.IOT.IOTDevices.Models.DB.ITApis;
using Core.Entities.IOT.IOTDevices.Models.DB.ITApiStations;
using Core.Entities.IOT.IOTDevices.Models.DB.OTCameras;
using Core.Entities.IOT.IOTDevices.Models.DB.OTTwinCats;
using Core.Entities.IOT.IOTDevices.Models.DB.ServerRules;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Entities.IOT.IOTTags.Models.DB.OTTagsTwinCat;
using Core.Entities.KPIData.KPIs.Models.DB;
using Core.Entities.KPIData.TenBestMatchs.Models.DB;
using Core.Entities.KPIData.WarningMsgs.Models.DB;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.AlarmLists;
using Core.Entities.Packets.Models.DB.Furnaces.InFurnaces;
using Core.Entities.Packets.Models.DB.Furnaces.OutFurnaces;
using Core.Entities.Packets.Models.DB.MetaDatas;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DB.LoadableCycles.S1S2Cycles;
using Core.Entities.StationCycles.Models.DB.MatchableCycles;
using Core.Entities.StationCycles.Models.DB.MatchableCycles.S3S4Cycles;
using Core.Entities.StationCycles.Models.DB.MatchableCycles.S5Cycles;
using Core.Entities.User.Models.DB.Acts;
using Core.Entities.User.Models.DB.Acts.ActEntities;
using Core.Entities.User.Models.DB.Acts.ActEntities.ActEntityRoles;
using Core.Entities.User.Models.DB.Roles;
using Core.Entities.User.Models.DB.Users;
using Core.Entities.Vision.Testing.Models.DB;
using Core.Entities.Vision.ToDos.Models.DB.Datasets;
using Core.Entities.Vision.ToDos.Models.DB.ToLoads;
using Core.Entities.Vision.ToDos.Models.DB.ToMatchs;
using Core.Entities.Vision.ToDos.Models.DB.ToNotifys;
using Core.Entities.Vision.ToDos.Models.DB.ToSigns;
using Core.Entities.Vision.ToDos.Models.DB.ToUnloads;
using Core.Shared.Models.DB.System.Logs;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Core.Shared.Data;

public class AnodeCTX : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
	public AnodeCTX(DbContextOptions<AnodeCTX> options) : base(options)
	{
	}

	public DbSet<LogEntry> Logs => Set<LogEntry>();

	public DbSet<BenchmarkTest> BenchmarkTest => Set<BenchmarkTest>();
	public DbSet<CameraTest> CameraTest => Set<CameraTest>();

	#region Vision

	public DbSet<ToMatch> ToMatch => Set<ToMatch>();
	public DbSet<ToLoad> ToLoad => Set<ToLoad>();
	public DbSet<ToSign> ToSign => Set<ToSign>();
	public DbSet<ToUnload> ToUnload => Set<ToUnload>();
	public DbSet<ToNotify> ToNotify => Set<ToNotify>();
	public DbSet<Dataset> Dataset => Set<Dataset>();

	#endregion Vision

	#region Alarms

	public DbSet<AlarmC> AlarmC => Set<AlarmC>();
	public DbSet<AlarmLog> AlarmLog => Set<AlarmLog>();
	public DbSet<AlarmRT> AlarmRT => Set<AlarmRT>();
	public DbSet<AlarmCycle> AlarmCycle => Set<AlarmCycle>();

	#endregion Alarms

	#region Anodes

	public DbSet<Anode> Anode => Set<Anode>();
	public DbSet<AnodeD20> AnodeD20 => Set<AnodeD20>();
	public DbSet<AnodeDX> AnodeDX => Set<AnodeDX>();

	#endregion Anodes

	#region Packets

	public DbSet<Packet> Packet => Set<Packet>();
	public DbSet<AlarmList> AlarmList => Set<AlarmList>();
	public DbSet<InFurnace> InFurnace => Set<InFurnace>();
	public DbSet<OutFurnace> OutFurnace => Set<OutFurnace>();
	public DbSet<MetaData> MetaData => Set<MetaData>();
	public DbSet<Shooting> Shooting => Set<Shooting>();

	#endregion Packets

	#region StationCycle

	public DbSet<StationCycle> StationCycle => Set<StationCycle>();
	public DbSet<S1S2Cycle> S1S2Cycle => Set<S1S2Cycle>();
	public DbSet<S3S4Cycle> S3S4Cycle => Set<S3S4Cycle>();
	public DbSet<S5Cycle> S5Cycle => Set<S5Cycle>();

	#endregion StationCycle

	#region IOT Monitoring

	public DbSet<IOTDevice> IOTDevice => Set<IOTDevice>();
	public DbSet<OTCamera> OTCamera => Set<OTCamera>();
	public DbSet<OTTwinCat> OTTwinCat => Set<OTTwinCat>();
	public DbSet<ITApi> ITApi => Set<ITApi>();
	public DbSet<ITApiStation> ITApiStations => Set<ITApiStation>();
	public DbSet<IOTTag> IOTTag => Set<IOTTag>();
	public DbSet<OTTagTwinCat> OTTagTwinCat => Set<OTTagTwinCat>();
	public DbSet<ServerRule> ServerRule => Set<ServerRule>();
	public DbSet<Sign> Sign => Set<Sign>();
	public DbSet<Match> Match => Set<Match>();

	#endregion IOT Monitoring

	#region KPI

	public DbSet<KPI> KPI => Set<KPI>();
	public DbSet<TenBestMatch> TenBestMatch => Set<TenBestMatch>();
	public DbSet<WarningMsg> WarningMsg => Set<WarningMsg>();
	public DbSet<BITemperature> BITemperature => Set<BITemperature>();

	#endregion KPI

	#region Action

	public DbSet<Act> Acts => Set<Act>();
	public DbSet<ActEntity> ActEntities => Set<ActEntity>();
	public DbSet<ActEntityRole> ActEntityRoles => Set<ActEntityRole>();

	#endregion Action

	#region Testing

	public DbSet<StationTest> StationTests => Set<StationTest>();

	#endregion Testing

	#region DebugMode

	public DbSet<DebugMode> DebugModes => Set<DebugMode>();

	#endregion DebugMode

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		// It is not needed to give the triggers' names. EFC just need to know if there is one since .NET 7
		builder.Entity<AlarmLog>().ToTable(tb => tb.HasTrigger("trigger"));

		builder.Entity<AlarmLog>()
			.HasOne(j => j.Alarm)
			.WithMany(a => a.AlarmLogs)
			.HasForeignKey(j => j.AlarmID)
			.OnDelete(DeleteBehavior.Cascade);

		builder.Entity<AlarmRT>()
			.HasOne(at => at.Alarm)
			.WithOne(ac => ac.AlarmRT)
			.HasForeignKey<AlarmRT>(at => at.AlarmID);

		builder.Entity<AlarmList>()
			.HasMany(alarmListPacket => alarmListPacket.AlarmCycles)
			.WithOne(alarmCycle => alarmCycle.AlarmList)
			.HasForeignKey(alarmCycle => alarmCycle.AlarmListPacketID)
			.IsRequired();

		builder.Entity<KPI>()
			.HasMany(kpi => kpi.TenBestMatches)
			.WithOne(tenBestMatch => tenBestMatch.KPI)
			.HasForeignKey(tenBestMatch => tenBestMatch.KPIID)
			.IsRequired();

		builder.Entity<KPI>()
			.HasMany(kpi => kpi.WarningMsgs)
			.WithOne(warningMsg => warningMsg.KPI)
			.HasForeignKey(warningMsg => warningMsg.KPIID)
			.IsRequired();

		builder.Entity<StationCycle>()
			.HasOne(stationCycle => stationCycle.MetaDataPacket)
			.WithOne(packet => packet.StationCycle)
			.HasForeignKey<StationCycle>(stationCycle => stationCycle.MetaDataID)
			.IsRequired(false)
			.OnDelete(DeleteBehavior.ClientSetNull);

		builder.Entity<StationCycle>()
			.HasOne(stationCycle => stationCycle.Shooting1Packet)
			.WithOne(packet => packet.StationCycle)
			.HasForeignKey<StationCycle>(stationCycle => stationCycle.Shooting1ID)
			.IsRequired(false)
			.OnDelete(DeleteBehavior.ClientSetNull);

		builder.Entity<StationCycle>()
			.HasOne(stationCycle => stationCycle.Shooting2Packet)
			.WithOne(packet => packet.StationCycleShooting2)
			.HasForeignKey<StationCycle>(stationCycle => stationCycle.Shooting2ID)
			.IsRequired(false)
			.OnDelete(DeleteBehavior.ClientSetNull);

		builder.Entity<StationCycle>()
			.HasOne(stationCycle => stationCycle.AlarmListPacket)
			.WithOne(packet => packet.StationCycle)
			.HasForeignKey<StationCycle>(stationCycle => stationCycle.AlarmListID)
			.IsRequired(false)
			.OnDelete(DeleteBehavior.ClientSetNull);

		builder.Entity<MatchableCycle>()
			.HasOne(stationCycle => stationCycle.KPI)
			.WithOne(kpi => kpi.StationCycle)
			.HasForeignKey<MatchableCycle>(stationCycle => stationCycle.KPIID)
			.IsRequired(false)
			.OnDelete(DeleteBehavior.ClientSetNull);

		builder.Entity<S3S4Cycle>()
			.HasOne(s3S4Cycle => s3S4Cycle.InFurnacePacket)
			.WithOne(packet => packet.StationCycle as S3S4Cycle)
			.HasForeignKey<S3S4Cycle>(s3S4Cycle => s3S4Cycle.InFurnaceID)
			.IsRequired(false)
			.OnDelete(DeleteBehavior.ClientSetNull);

		builder.Entity<S3S4Cycle>()
			.HasOne(s3S4Cycle => s3S4Cycle.OutFurnacePacket)
			.WithOne(packet => packet.StationCycle as S3S4Cycle)
			.HasForeignKey<S3S4Cycle>(s3S4Cycle => s3S4Cycle.OutFurnaceID)
			.IsRequired(false)
			.OnDelete(DeleteBehavior.ClientSetNull);

		builder.Entity<S5Cycle>()
			.HasOne(s5cycle => s5cycle.ChainCycle)
			.WithOne(s3s4 => s3s4.ChainCycle)
			.HasForeignKey<S5Cycle>(cycle => cycle.ChainCycleID)
			.IsRequired(false)
			.OnDelete(DeleteBehavior.ClientSetNull);

		builder.Entity<IOTDevice>()
			.HasMany(iotDevice => iotDevice.IOTTags)
			.WithOne(iotTag => iotTag.IOTDevice)
			.HasForeignKey(iotTag => iotTag.IOTDeviceID);

		builder.Entity<Anode>()
			.HasOne(anode => anode.S1S2Cycle)
			.WithOne(cycle => cycle.Anode)
			.HasForeignKey<Anode>(anode => anode.S1S2CycleID);

		builder.Entity<Anode>()
			.HasOne(anode => anode.S3S4Cycle)
			.WithOne(cycle => cycle.Anode)
			.HasForeignKey<Anode>(anode => anode.S3S4CycleID)
			.OnDelete(DeleteBehavior.NoAction);

		builder.Entity<AnodeDX>()
			.HasOne(anode => anode.S5Cycle)
			.WithOne(cycle => cycle.Anode)
			.HasForeignKey<AnodeDX>(anode => anode.S5CycleID)
			.OnDelete(DeleteBehavior.NoAction);

		builder.Entity<BenchmarkTest>()
			.HasIndex(b => b.TSIndex);

		builder.Entity<CameraTest>()
			.HasMany(c => c.BenchmarkTests)
			.WithOne(b => b.CameraTest)
			.HasForeignKey(b => b.CameraID);
	}
}