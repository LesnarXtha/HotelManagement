using HotelManagement.Bookings;
using HotelManagement.Rooms;
using HotelManagement.Users;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Data
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options) { }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<BookingModel> Bookings { get; set; }
        public DbSet<UserModel> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed the Users table with 3 users
            modelBuilder.Entity<UserModel>().HasData(
                new UserModel
                {
                    Id = 1,
                    Email = "admin@hotel.com",
                    Password = "admin123",
                },
                new UserModel
                {
                    Id = 2,
                    Email = "user1@hotel.com",
                    Password = "user123",
                },
                new UserModel
                {
                    Id = 3,
                    Email = "user2@hotel.com",
                    Password = "user456",
                }
            );
        }
    }
}
