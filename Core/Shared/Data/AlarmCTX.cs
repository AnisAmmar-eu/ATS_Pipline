using Core.Entities.AlarmsC.Models.DB;
using Core.Entities.AlarmsPLC.Models.DB;
using Microsoft.EntityFrameworkCore;
using AlarmLog = Core.Entities.AlarmsLog.Models.DB.AlarmLog;
using AlarmRT = Core.Entities.AlarmsRT.Models.DB.AlarmRT;

namespace Core.Shared.Data;

public class AlarmCTX : DbContext
{
	public AlarmCTX(DbContextOptions<AlarmCTX> options) : base(options)
	{
	}


	public DbSet<AlarmPLC> AlarmPLC { get; set; }
	public DbSet<AlarmC> AlarmC { get; set; }
	public DbSet<AlarmLog> AlarmLog { get; set; }
	public DbSet<AlarmRT> AlarmRT { get; set; }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<AlarmLog>()
			.HasOne(j => j.Alarm)
			.WithMany(a => a.AlarmLogs)
			.HasForeignKey(j => j.AlarmID)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<AlarmRT>()
			.HasOne(at => at.Alarm)
			.WithOne(ac => ac.AlarmRT)
			.HasForeignKey<AlarmRT>(at => at.AlarmID);
	}
}