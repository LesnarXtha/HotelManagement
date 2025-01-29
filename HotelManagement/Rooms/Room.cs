using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Rooms
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Room number is required.")]
        public int RoomNumber { get; set; }

        [Required(ErrorMessage = "Room price is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal RoomPrice { get; set; }

        [Required(ErrorMessage = "Room type is required.")]
        public string RoomType { get; set; } // "Deluxe" or "Standard"

        [Required(ErrorMessage = "Room availability is required.")]
        public bool RoomAvailability { get; set; } // Available or Not Available

        [Required(ErrorMessage = "Please upload a picture.")]
        public string PicturePath { get; set; } // Path to the uploaded picture

        public List<string> Amenities { get; set; } // Stores the selected checkboxes (e.g., WiFi, Breakfast, etc.)
    }
}
