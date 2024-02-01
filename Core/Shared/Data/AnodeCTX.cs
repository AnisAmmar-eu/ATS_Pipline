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
using Core.Entities.IOT.IOTDevices.Models.DB;
using Core.Entities.IOT.IOTDevices.Models.DB.ITApis;
using Core.Entities.IOT.IOTDevices.Models.DB.OTCameras;
using Core.Entities.IOT.IOTDevices.Models.DB.OTTwinCats;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Entities.IOT.IOTTags.Models.DB.OTTagsTwinCat;
using Core.Entities.KPI.KPICs.Models.DB;
using Core.Entities.KPI.KPIEntries.Models.DB.KPILogs;
using Core.Entities.KPI.KPIEntries.Models.DB.KPIRTs;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.AlarmLists;
using Core.Entities.Packets.Models.DB.Furnaces.InFurnaces;
using Core.Entities.Packets.Models.DB.Furnaces.OutFurnaces;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.Packets.Models.DB.Shootings.S3S4Shootings;
using Core.Entities.Packets.Models.DB.Shootings.S5Shootings;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DB.LoadableCycles.S1S2Cycles;
using Core.Entities.StationCycles.Models.DB.MatchableCycles.S3S4Cycles;
using Core.Entities.StationCycles.Models.DB.MatchableCycles.S5Cycles;
using Core.Entities.User.Models.DB.Acts;
using Core.Entities.User.Models.DB.Acts.ActEntities;
using Core.Entities.User.Models.DB.Acts.ActEntities.ActEntityRoles;
using Core.Entities.User.Models.DB.Roles;
using Core.Entities.User.Models.DB.Users;
using Core.Entities.Vision.FileSettings.Models.DB;
using Core.Entities.Vision.SignedCycles.Models.DB.LoadableQueues;
using Core.Entities.Vision.SignedCycles.Models.DB.MatchableStacks;
using Core.Shared.Models.DB.System.Logs;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Core.Shared.Data;

public class AnodeCTX : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
	public AnodeCTX(DbContextOptions<AnodeCTX> options) : base(options)
	{
	}

	public DbSet<Log> Log => Set<Log>();

	public DbSet<BenchmarkTest> BenchmarkTest => Set<BenchmarkTest>();
	public DbSet<CameraTest> CameraTest => Set<CameraTest>();

	#region Vision

	public DbSet<FileSetting> FileSetting => Set<FileSetting>();
	public DbSet<MatchableStack> MatchableStack => Set<MatchableStack>();
	public DbSet<LoadableQueue> LoadableQueue => Set<LoadableQueue>();

	#endregion

	#region Alarms

	public DbSet<AlarmC> AlarmC => Set<AlarmC>();
	public DbSet<AlarmLog> AlarmLog => Set<AlarmLog>();
	public DbSet<AlarmRT> AlarmRT => Set<AlarmRT>();
	public DbSet<AlarmCycle> AlarmCycle => Set<AlarmCycle>();

	#endregion

	#region Anodes

	public DbSet<Anode> Anode => Set<Anode>();
	public DbSet<AnodeD20> AnodeD20 => Set<AnodeD20>();
	public DbSet<AnodeDX> AnodeDX => Set<AnodeDX>();

	#endregion

	#region Packets

	public DbSet<Packet> Packet => Set<Packet>();
	public DbSet<AlarmList> AlarmList => Set<AlarmList>();
	public DbSet<InFurnace> InFurnace => Set<InFurnace>();
	public DbSet<OutFurnace> OutFurnace => Set<OutFurnace>();
	public DbSet<Shooting> Shooting => Set<Shooting>();
	public DbSet<S3S4Shooting> S3S4Shooting => Set<S3S4Shooting>();
	public DbSet<S5Shooting> S5Shooting => Set<S5Shooting>();

	#endregion

	#region StationCycle

	public DbSet<StationCycle> StationCycle => Set<StationCycle>();
	public DbSet<S1S2Cycle> S1S2Cycle => Set<S1S2Cycle>();
	public DbSet<S3S4Cycle> S3S4Cycle => Set<S3S4Cycle>();
	public DbSet<S5Cycle> S5Cycle => Set<S5Cycle>();

	#endregion

	#region IOT Monitoring

	public DbSet<IOTDevice> IOTDevice => Set<IOTDevice>();
	public DbSet<OTCamera> OTCamera => Set<OTCamera>();
	public DbSet<OTTwinCat> OTTwinCat => Set<OTTwinCat>();
	public DbSet<ITApi> ITApi => Set<ITApi>();
	public DbSet<IOTTag> IOTTag => Set<IOTTag>();
	public DbSet<OTTagTwinCat> OTTagTwinCat => Set<OTTagTwinCat>();

	#endregion

	#region KPI

	public DbSet<KPIC> KPIC => Set<KPIC>();
	public DbSet<KPILog> KPILog => Set<KPILog>();
	public DbSet<KPIRT> KPIRT => Set<KPIRT>();
	public DbSet<BITemperature> BITemperature => Set<BITemperature>();

	#endregion

	#region Action

	public DbSet<Act> Acts => Set<Act>();
	public DbSet<ActEntity> ActEntities => Set<ActEntity>();
	public DbSet<ActEntityRole> ActEntityRoles => Set<ActEntityRole>();

	#endregion

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

		builder.Entity<KPILog>()
			.HasOne(kpiLog => kpiLog.KPIC)
			.WithMany(kpic => kpic.LogEntries)
			.HasForeignKey(kpiLog => kpiLog.KPICID)
			.OnDelete(DeleteBehavior.Cascade);

		builder.Entity<KPIRT>()
			.HasOne(kpiRT => kpiRT.KPIC)
			.WithMany(kpic => kpic.RTEntries)
			.HasForeignKey(kpiRT => kpiRT.KPICID)
			.OnDelete(DeleteBehavior.Cascade);

		builder.Entity<StationCycle>()
			.HasOne(stationCycle => stationCycle.ShootingPacket)
			.WithOne(packet => packet.StationCycle)
			.HasForeignKey<StationCycle>(stationCycle => stationCycle.ShootingID)
			.IsRequired(false)
			.OnDelete(DeleteBehavior.NoAction);

		builder.Entity<StationCycle>()
			.HasOne(stationCycle => stationCycle.AlarmListPacket)
			.WithOne(packet => packet.StationCycle)
			.HasForeignKey<StationCycle>(stationCycle => stationCycle.AlarmListID)
			.IsRequired(false)
			.OnDelete(DeleteBehavior.NoAction);

		builder.Entity<S3S4Cycle>()
			.HasOne(s3S4Cycle => s3S4Cycle.InFurnacePacket)
			.WithOne(packet => packet.StationCycle as S3S4Cycle)
			.HasForeignKey<S3S4Cycle>(s3S4Cycle => s3S4Cycle.InFurnaceID)
			.IsRequired(false)
			.OnDelete(DeleteBehavior.NoAction);

		builder.Entity<S3S4Cycle>()
			.HasOne(s3S4Cycle => s3S4Cycle.OutFurnacePacket)
			.WithOne(packet => packet.StationCycle as S3S4Cycle)
			.HasForeignKey<S3S4Cycle>(s3S4Cycle => s3S4Cycle.OutFurnaceID)
			.IsRequired(false)
			.OnDelete(DeleteBehavior.NoAction);

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

		builder.Entity<MatchableStack>()
			.HasOne(matchableStack => matchableStack.MatchableCycle)
			.WithMany(matchableCycle => matchableCycle.MatchableStacks)
			.HasForeignKey(matchableStack => matchableStack.MatchableCycleID);

		builder.Entity<MatchableStack>()
			.HasIndex(matchableStack => matchableStack.CycleTS);

		builder.Entity<LoadableQueue>()
			.HasOne(loadableQueue => loadableQueue.LoadableCycle)
			.WithMany(loadableCycle => loadableCycle.LoadableQueues)
			.HasForeignKey(loadableQueue => loadableQueue.LoadableCycleID);

		builder.Entity<LoadableQueue>()
			.HasIndex(loadableQueue => loadableQueue.CycleTS);
	}
}