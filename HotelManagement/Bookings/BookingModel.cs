using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotelManagement.Rooms;

namespace HotelManagement.Bookings
{
    public class BookingModel
    {
        [Key] // Marks as primary key
        public int Id { get; set; }

        [Required] // Ensures the field is mandatory
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string FirstName { get; set; }

        [Required] // Ensures the field is mandatory
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string LastName { get; set; }

        [Required] // Ensures CheckInDate is mandatory
        [DataType(DataType.Date)] // Specifies the data type for display/validation
        public DateTime CheckInDate { get; set; }

        [Required] // Ensures CheckOutDate is mandatory
        [DataType(DataType.Date)] // Specifies the data type for display/validation
        [Display(Name = "Check-Out Date")] // Custom display name for UI
        public DateTime CheckOutDate { get; set; }

        // Foreign key to Room
        [ForeignKey(nameof(Room))] // Maps to the Room entity
        public int RoomId { get; set; }

        public Room? Room { get; set; } // Navigation property for the Room
    }
}
