namespace API.Data
{
    using Microsoft.EntityFrameworkCore;
    using API.Model;
    using Microsoft.Identity.Client;
    using backend.API.Model;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){ }
        public DbSet<Patient> Patients { get; set;  }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<JournalEntry> JournalEntries { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Perscription> Perscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>().HasBaseType<AbstractUser>();
            modelBuilder.Entity<Patient>().HasBaseType<AbstractUser>();

            modelBuilder.Entity<JournalEntry>()
                .HasOne(j => j.Doctor)
                .WithMany(d => d.JournalEntries)
                .HasForeignKey(j => j.DoctorID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JournalEntry>()
                .HasOne(j => j.Patient)
                .WithMany(p => p.JournalEntries)
                .HasForeignKey(j => j.PatientID)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Perscription>()
                .HasOne(p => p.Patient)
                .WithMany(p => p.Perscriptions)
                .HasForeignKey(p => p.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Perscription>()
               .HasOne(p => p.Doctor)
               .WithMany(d => d.Perscriptions)
               .HasForeignKey(p => p.DoctorId)
               .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
