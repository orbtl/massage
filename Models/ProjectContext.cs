using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace massage.Models
{
    public class ProjectContext : IdentityDbContext<User>
    {
        public ProjectContext(DbContextOptions options) : base(options){}
        public new DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Insurance> Insurances { get; set; }
        public DbSet<PAvailTime> PAvailTimes { get; set; }
        public DbSet<PInsurance> PInsurances { get; set; }
        public DbSet<PSchedule> PSchedules { get; set; }
        public DbSet<PService> PServices { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomService> RoomServices { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Timeslot> Timeslots { get; set; }
        
    }
}