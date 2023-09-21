using System.ComponentModel.DataAnnotations;
using Core.Entities.AlarmsC.Models.DB;
using Core.Entities.AlarmsCycle.Models.DB;
using Core.Entities.AlarmsLog.Repositories;
using Core.Entities.AlarmsPLC.Models.DB;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.Detection;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.EntityFrameworkCore;
using AlarmListPacket = Core.Entities.Packets.Models.DB.AlarmListPackets.AlarmListPacket;
using AlarmLog = Core.Entities.AlarmsLog.Models.DB.AlarmLog;
using AlarmRT = Core.Entities.AlarmsRT.Models.DB.AlarmRT;

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
	public DbSet<AlarmListPacket> AlarmListPacket => Set<AlarmListPacket>();
	public DbSet<DetectionPacket> DetectionPacket => Set<DetectionPacket>();


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

		modelBuilder.Entity<AlarmListPacket>()
			.HasMany(alarmListPacket => alarmListPacket.AlarmList)
			.WithOne(alarmCycle => alarmCycle.AlarmListPacket)
			.HasForeignKey(alarmCycle => alarmCycle.AlarmListPacketID)
			.IsRequired();
	}
}