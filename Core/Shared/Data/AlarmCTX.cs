using System.ComponentModel.DataAnnotations;
using Core.Entities.Packets.Models.DB;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.EntityFrameworkCore;
using Core.Entities.Alarms.AlarmsC.Models.DB;
using Core.Entities.Alarms.AlarmsCycle.Models.DB;
using Core.Entities.Alarms.AlarmsLog.Models.DB;
using Core.Entities.Alarms.AlarmsPLC.Models.DB;
using Core.Entities.Alarms.AlarmsRT.Models.DB;
using Core.Entities.Packets.Models.DB.AlarmLists;
using Core.Entities.Packets.Models.DB.Detections;

namespace Core.Shared.Data;

public class AlarmCTX : DbContext
{
	public AlarmCTX(DbContextOptions<AlarmCTX> options) : base(options)
	{
	}

	// Alarms
	public DbSet<AlarmPLC> AlarmPLC => Set<AlarmPLC>();
	public DbSet<AlarmC> AlarmC => Set<AlarmC>();
	public DbSet<AlarmLog> AlarmLog => Set<AlarmLog>();
	public DbSet<AlarmRT> AlarmRT => Set<AlarmRT>();
	public DbSet<AlarmCycle> AlarmCycle => Set<AlarmCycle>();
	
	// Packets
	public DbSet<Packet> Packet => Set<Packet>();
	public DbSet<Entities.Packets.Models.DB.AlarmLists.AlarmList> AlarmListPacket => Set<Entities.Packets.Models.DB.AlarmLists.AlarmList>();
	public DbSet<Detection> DetectionPacket => Set<Detection>();


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
	}
}