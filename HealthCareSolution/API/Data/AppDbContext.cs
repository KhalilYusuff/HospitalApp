namespace API.Data
{
    using Microsoft.EntityFrameworkCore;
    using API.model;
    using Microsoft.Identity.Client;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){ }
        public DbSet<Patient> Patients { get; set;  }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<JournalEntry> JournalEntries { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>().HasBaseType<AbstractUser>();
            modelBuilder.Entity<Patient>().HasBaseType<AbstractUser>();

            modelBuilder.Entity<JournalEntry>()
                .HasOne(j => j.Doctor)
                .WithMany(d => d.JournalEntries)
                .HasForeignKey(j => j.DoctorID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JournalEntry>()
                .HasOne(j => j.Patient)
                .WithMany(p => p.JournalEntries)
                .HasForeignKey(j => j.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            // Fix for CS0029 and CS1662
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
