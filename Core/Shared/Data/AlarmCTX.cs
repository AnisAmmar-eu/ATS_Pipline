using Core.Entities.AlarmsC.Models.DB;
using Core.Entities.AlarmsPLC.Models.DB;
using Core.Entities.Journals.Models.DB;
using Microsoft.EntityFrameworkCore;
using AlarmRT = Core.Entities.AlarmsRT.Models.DB.AlarmRT;

namespace Core.Shared.Data;

public class AlarmCTX : DbContext
{
    public AlarmCTX(DbContextOptions<AlarmCTX> options) : base(options)
    {
    }


    public DbSet<AlarmPLC> AlarmPLC { get; set; }
    public DbSet<AlarmC> AlarmC { get; set; }
    public DbSet<Journal> Journal { get; set; }
    public DbSet<AlarmRT> AlarmRT { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Journal>()
            .HasOne(j => j.Alarm)
            .WithMany(a => a.Journals)
            .HasForeignKey(j => j.IDAlarm)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AlarmRT>()
            .HasOne(at => at.AlarmC)
            .WithOne(ac => ac.AlarmRT)
            .HasForeignKey<AlarmRT>(at => at.IDAlarm);
    }
}