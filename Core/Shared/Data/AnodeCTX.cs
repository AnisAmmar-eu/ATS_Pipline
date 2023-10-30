﻿using Core.Entities.Alarms.AlarmsC.Models.DB;
using Core.Entities.Alarms.AlarmsCycle.Models.DB;
using Core.Entities.Alarms.AlarmsLog.Models.DB;
using Core.Entities.Alarms.AlarmsPLC.Models.DB;
using Core.Entities.Alarms.AlarmsRT.Models.DB;
using Core.Entities.BI.BITemperatures.Models.DB;
using Core.Entities.IOT.IOTDevices.Models.DB;
using Core.Entities.IOT.IOTDevices.Models.DB.ITApis;
using Core.Entities.IOT.IOTDevices.Models.DB.OTCameras;
using Core.Entities.IOT.IOTDevices.Models.DB.OTRockwells;
using Core.Entities.IOT.IOTDevices.Models.DB.OTTwinCats;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Entities.IOT.IOTTags.Models.DB.OTTagsTwinCat;
using Core.Entities.KPI.KPICs.Models.DB;
using Core.Entities.KPI.KPIEntries.Models.DB.KPILogs;
using Core.Entities.KPI.KPIEntries.Models.DB.KPIRTs;
using Core.Entities.KPI.KPITests.Models.DB;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.AlarmLists;
using Core.Entities.Packets.Models.DB.Announcements;
using Core.Entities.Packets.Models.DB.Announcements.S1S2Announcement;
using Core.Entities.Packets.Models.DB.Detections;
using Core.Entities.Packets.Models.DB.Furnaces.InFurnaces;
using Core.Entities.Packets.Models.DB.Furnaces.OutFurnaces;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.Packets.Models.DB.Shootings.S3S4Shootings;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DB.S1S2Cycles;
using Core.Entities.StationCycles.Models.DB.S3S4Cycles;
using Core.Entities.StationCycles.Models.DB.S5Cycles;
using Core.Entities.User.Models.DB.Acts;
using Core.Entities.User.Models.DB.Acts.ActEntities;
using Core.Entities.User.Models.DB.Acts.ActEntities.ActEntityRoles;
using Core.Entities.User.Models.DB.Roles;
using Core.Entities.User.Models.DB.Users;
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

	// Alarms
	public DbSet<AlarmPLC> AlarmPLC => Set<AlarmPLC>();
	public DbSet<AlarmC> AlarmC => Set<AlarmC>();
	public DbSet<AlarmLog> AlarmLog => Set<AlarmLog>();
	public DbSet<AlarmRT> AlarmRT => Set<AlarmRT>();
	public DbSet<AlarmCycle> AlarmCycle => Set<AlarmCycle>();

	// Packets
	public DbSet<Packet> Packet => Set<Packet>();
	public DbSet<AlarmList> AlarmList => Set<AlarmList>();
	public DbSet<Announcement> Announcement => Set<Announcement>();
	public DbSet<S1S2Announcement> S1S2Announcement => Set<S1S2Announcement>();
	public DbSet<Detection> Detection => Set<Detection>();
	public DbSet<InFurnace> InFurnace => Set<InFurnace>();
	public DbSet<OutFurnace> OutFurnace => Set<OutFurnace>();
	public DbSet<Shooting> Shooting => Set<Shooting>();
	public DbSet<S3S4Shooting> S3S4Shooting => Set<S3S4Shooting>();

	// StationCycle
	public DbSet<StationCycle> StationCycle => Set<StationCycle>();
	public DbSet<S1S2Cycle> S1S2Cycle => Set<S1S2Cycle>();
	public DbSet<S3S4Cycle> S3S4Cycle => Set<S3S4Cycle>();
	public DbSet<S5Cycle> S5Cycle => Set<S5Cycle>();

	// IOT Monitoring
	public DbSet<IOTDevice> IOTDevice => Set<IOTDevice>();
	public DbSet<OTCamera> OTCamera => Set<OTCamera>();
	public DbSet<OTTwinCat> OTTwinCat => Set<OTTwinCat>();
	public DbSet<ITApi> ITApi => Set<ITApi>();
	public DbSet<OTRockwell> OTRockwell => Set<OTRockwell>();
	public DbSet<IOTTag> IOTTag => Set<IOTTag>();
	public DbSet<OTTagTwinCat> OTTagTwinCat => Set<OTTagTwinCat>();

	// KPI
	public DbSet<KPIC> KPIC => Set<KPIC>();
	public DbSet<KPILog> KPILog => Set<KPILog>();
	public DbSet<KPIRT> KPIRT => Set<KPIRT>();
	public DbSet<KPITest> KPITest => Set<KPITest>();
	public DbSet<BITemperature> BITemperature => Set<BITemperature>();

	// Action
	public DbSet<Act> Acts => Set<Act>();
	public DbSet<ActEntity> ActEntities => Set<ActEntity>();
	public DbSet<ActEntityRole> ActEntityRoles => Set<ActEntityRole>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<AlarmLog>()
			.HasOne(j => j.Alarm)
			.WithMany(a => a.AlarmLogs)
			.HasForeignKey(j => j.AlarmID)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<AlarmRT>()
			.HasOne(at => at.Alarm)
			.WithOne(ac => ac.AlarmRT)
			.HasForeignKey<AlarmRT>(at => at.AlarmID);

		modelBuilder.Entity<AlarmList>()
			.HasMany(alarmListPacket => alarmListPacket.AlarmCycles)
			.WithOne(alarmCycle => alarmCycle.AlarmList)
			.HasForeignKey(alarmCycle => alarmCycle.AlarmListPacketID)
			.IsRequired();

		modelBuilder.Entity<KPILog>()
			.HasOne(kpiLog => kpiLog.KPIC)
			.WithMany(kpic => kpic.LogEntries)
			.HasForeignKey(kpiLog => kpiLog.KPICID)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<KPIRT>()
			.HasOne(kpiRT => kpiRT.KPIC)
			.WithMany(kpic => kpic.RTEntries)
			.HasForeignKey(kpiRT => kpiRT.KPICID)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<StationCycle>()
			.HasOne(stationCycle => stationCycle.AnnouncementPacket)
			.WithOne(packet => packet.StationCycle)
			.HasForeignKey<StationCycle>(stationCycle => stationCycle.AnnouncementID)
			.IsRequired(false)
			.OnDelete(DeleteBehavior.NoAction);

		modelBuilder.Entity<StationCycle>()
			.HasOne(stationCycle => stationCycle.DetectionPacket)
			.WithOne(packet => packet.StationCycle)
			.HasForeignKey<StationCycle>(stationCycle => stationCycle.DetectionID)
			.IsRequired(false)
			.OnDelete(DeleteBehavior.NoAction);

		modelBuilder.Entity<StationCycle>()
			.HasOne(stationCycle => stationCycle.ShootingPacket)
			.WithOne(packet => packet.StationCycle)
			.HasForeignKey<StationCycle>(stationCycle => stationCycle.ShootingID)
			.IsRequired(false)
			.OnDelete(DeleteBehavior.NoAction);

		modelBuilder.Entity<StationCycle>()
			.HasOne(stationCycle => stationCycle.AlarmListPacket)
			.WithOne(packet => packet.StationCycle)
			.HasForeignKey<StationCycle>(stationCycle => stationCycle.AlarmListID)
			.IsRequired(false)
			.OnDelete(DeleteBehavior.NoAction);

		modelBuilder.Entity<S1S2Cycle>()
			.HasOne(s1S2Cycle => s1S2Cycle.AnnouncementPacket)
			.WithOne(packet => packet.StationCycle as S1S2Cycle)
			.HasForeignKey<S1S2Cycle>(s1S2Cycle => s1S2Cycle.AnnouncementID)
			.IsRequired(false)
			.OnDelete(DeleteBehavior.NoAction);

		modelBuilder.Entity<S3S4Cycle>()
			.HasOne(s3S4Cycle => s3S4Cycle.InFurnacePacket)
			.WithOne(packet => packet.StationCycle as S3S4Cycle)
			.HasForeignKey<S3S4Cycle>(s3S4Cycle => s3S4Cycle.InFurnaceID)
			.IsRequired(false)
			.OnDelete(DeleteBehavior.NoAction);

		modelBuilder.Entity<S3S4Cycle>()
			.HasOne(s3S4Cycle => s3S4Cycle.OutFurnacePacket)
			.WithOne(packet => packet.StationCycle as S3S4Cycle)
			.HasForeignKey<S3S4Cycle>(s3S4Cycle => s3S4Cycle.OutFurnaceID)
			.IsRequired(false)
			.OnDelete(DeleteBehavior.NoAction);

		modelBuilder.Entity<IOTDevice>()
			.HasMany(iotDevice => iotDevice.IOTTags)
			.WithOne(iotTag => iotTag.IOTDevice)
			.HasForeignKey(iotTag => iotTag.IOTDeviceID);
	}
}