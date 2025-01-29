using HotelManagement.Bookings;
using HotelManagement.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Controllers
{
    public class BookingController : Controller
    {
        private readonly HotelDbContext _context;
        public BookingController(HotelDbContext context)
        {
            _context = context;
        }

        public IActionResult Details()
        {
            var bookings = _context.Bookings
                .Include(b => b.Room) // Include Room details
                .ToList();

            return View(bookings);
        }
        [HttpPost]
        public IActionResult Cancel(int id)
        {
            var booking = _context.Bookings
                .Include(b => b.Room) // Include Room details
                .FirstOrDefault(b => b.Id == id);

            if (booking == null)
            {
                return NotFound("Booking not found.");
            }

            // Mark the room as available
            if (booking.Room != null)
            {
                booking.Room.RoomAvailability = true;
            }

            // Remove the booking from the database
            _context.Bookings.Remove(booking);
            _context.SaveChanges();

            return RedirectToAction("Details"); // Redirect back to the list of bookings
        }

        public IActionResult Create(int roomId)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Id == roomId);
            if (room == null || !room.RoomAvailability)
            {
                return NotFound("Room not found or unavailable.");
            }

            var bookingModel = new BookingModel
            {
                RoomId = roomId,
                Room = room // Prepopulate the booking form with room details
            };

            return View(bookingModel);
        }

        [HttpPost]
        public IActionResult Create(BookingModel booking)
        {
            if (ModelState.IsValid)
            {
                // Save booking to the database
                _context.Bookings.Add(booking);

                // Mark the room as unavailable
                var room = _context.Rooms.FirstOrDefault(r => r.Id == booking.RoomId);
                if (room != null)
                {
                    room.RoomAvailability = false;
                    _context.SaveChanges();
                }

                _context.SaveChanges();
                return RedirectToAction("Index", "Room");
            }

            return View(booking);
        }
    }
}
