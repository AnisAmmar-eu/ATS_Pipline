using Core.Entities.Alarmes_C.Models.DB;
using Core.Entities.AlarmesPLC.Models.DB;
using Core.Entities.AlarmesTR.Models.DB;
using Core.Entities.Journals.Models.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Core.Shared.Data
{
    public class AlarmesDbContext : DbContext
    {
        public AlarmesDbContext(DbContextOptions<AlarmesDbContext> options) : base(options)
        {
        }


        public DbSet<AlarmePLC> AlarmePLC { get; set; }
        public DbSet<Alarme_C> Alarme_C { get; set; }
        public DbSet<Journal> Journal { get; set; }       
        public DbSet<AlarmeTR> AlarmeTR { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Journal>()
           .HasOne(j => j.Alarme)
           .WithMany(a => a.Journaux)
           .HasForeignKey(j => j.IdAlarme)
           .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AlarmeTR>()
          .HasOne(at => at.Alarme_C)
          .WithOne(ac => ac.AlarmeTR)
          .HasForeignKey<AlarmeTR>(at => at.IdAlarme);


        }



    }
}
