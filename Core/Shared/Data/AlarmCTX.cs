using Core.Entities.Alarms.AlarmsC.Models.DB;
using Core.Entities.Alarms.AlarmsCycle.Models.DB;
using Core.Entities.Alarms.AlarmsLog.Models.DB;
using Core.Entities.Alarms.AlarmsPLC.Models.DB;
using Core.Entities.Alarms.AlarmsRT.Models.DB;
using Core.Entities.BIPeriodicLogs.Models.DB;
using Core.Entities.BIPeriodicLogs.Models.DB.Entries.AnnualEntries;
using Core.Entities.BIPeriodicLogs.Models.DB.Entries.DailyEntries;
using Core.Entities.BIPeriodicLogs.Models.DB.Entries.MonthlyEntries;
using Core.Entities.BIPeriodicLogs.Models.DB.Entries.WeeklyEntries;
using Core.Entities.BIPeriodicLogs.Models.DB.RT.AnnualRTs;
using Core.Entities.BIPeriodicLogs.Models.DB.RT.DailyRTs;
using Core.Entities.BIPeriodicLogs.Models.DB.RT.MonthlyRTs;
using Core.Entities.BIPeriodicLogs.Models.DB.RT.WeeklyRTs;
using Core.Entities.ExtTags.Models.DB;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.AlarmLists;
using Core.Entities.Packets.Models.DB.Announcements;
using Core.Entities.Packets.Models.DB.Announcements.S1S2Announcement;
using Core.Entities.Packets.Models.DB.Detections;
using Core.Entities.Packets.Models.DB.Furnaces;
using Core.Entities.Packets.Models.DB.Furnaces.InFurnaces;
using Core.Entities.Packets.Models.DB.Furnaces.OutFurnaces;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.Packets.Models.DB.Shootings.S3S4Shootings;
using Core.Entities.ServicesMonitors.Models.DB;
using Core.Entities.User.Models.DB;
using Core.Entities.User.Models.DB.Acts;
using Core.Entities.User.Models.DB.Acts.ActEntities;
using Core.Entities.User.Models.DB.Acts.ActEntities.ActEntityRoles;
using Core.Entities.User.Models.DB.Roles;
using Core.Entities.User.Models.DB.Users;
using Core.Shared.Models.DB.System.Logs;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Core.Shared.Data;

public class AlarmCTX : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
	public AlarmCTX(DbContextOptions<AlarmCTX> options) : base(options)
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
	public DbSet<S1S2Announcement> S1S2Announcements => Set<S1S2Announcement>();
	public DbSet<Detection> Detection => Set<Detection>();
	public DbSet<InFurnace> InFurnace => Set<InFurnace>();
	public DbSet<OutFurnace> OutFurnace => Set<OutFurnace>();
	public DbSet<Shooting> Shooting => Set<Shooting>();
	public DbSet<S3S4Shooting> S3S4Shooting => Set<S3S4Shooting>();

	public DbSet<ExtTag> ExtTag => Set<ExtTag>();
	public DbSet<ServicesMonitor> ServicesMonitor => Set<ServicesMonitor>();
	
	// BIPeriodicLogs
	public DbSet<BIPeriodicLog> BIPeriodicLog => Set<BIPeriodicLog>();
	public DbSet<DailyEntry> DailyEntry => Set<DailyEntry>();
	public DbSet<WeeklyEntry> WeeklyEntry => Set<WeeklyEntry>();
	public DbSet<MonthlyEntry> MonthlyEntry => Set<MonthlyEntry>();
	public DbSet<AnnualEntry> AnnualEntry => Set<AnnualEntry>();
	public DbSet<DailyRT> DailyRT => Set<DailyRT>();
	public DbSet<WeeklyRT> WeeklyRT => Set<WeeklyRT>();
	public DbSet<MonthlyRT> MonthlyRT => Set<MonthlyRT>();
	public DbSet<AnnualRT> AnnualRT => Set<AnnualRT>();

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

		modelBuilder.Entity<ServicesMonitor>()
			.HasMany(servicesMonitor => servicesMonitor.ExtTags)
			.WithOne(extTag => extTag.Service)
			.HasForeignKey(extTag => extTag.ServiceID)
			.IsRequired();
	}
}