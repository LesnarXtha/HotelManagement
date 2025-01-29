using HotelManagement.Data;
using HotelManagement.Models;
using HotelManagement.Rooms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Controllers
{
    public class RoomController : Controller
    {
        private readonly HotelDbContext _context;

        public RoomController(HotelDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var rooms = _context.Rooms.ToList(); // Retrieve all rooms from the database
            return View(rooms); // Pass rooms to the view
        }
        public IActionResult Details(int id)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Id == id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Room room, IFormFile pictureFile)
        {
            if (!ModelState.IsValid)
            {
                // Handle file upload
                if (pictureFile != null)
                {
                    var filePath = Path.Combine("wwwroot/images", pictureFile.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        pictureFile.CopyTo(stream);
                    }
                    room.PicturePath = $"/images/{pictureFile.FileName}";
                }

                // Save to database
                _context.Rooms.Add(room);
                _context.SaveChanges();

                // Redirect to the Index action of the RoomController
                return RedirectToAction("Index", "Room");
            }

            return View(room);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Edit(int id)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Id == id);
            if (room == null)
            {
                return NotFound("Room not found.");
            }

            return View(room);
        }
        [HttpPost]
        public IActionResult Edit(int id, Room room, IFormFile? PictureFile)
        {
            if (id != room.Id)
            {
                return BadRequest("Room ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                var existingRoom = _context.Rooms.FirstOrDefault(r => r.Id == id);
                if (existingRoom == null)
                {
                    return NotFound("Room not found.");
                }

                existingRoom.RoomNumber = room.RoomNumber;
                existingRoom.RoomPrice = room.RoomPrice;
                existingRoom.RoomType = room.RoomType;
                existingRoom.RoomAvailability = room.RoomAvailability;

                // Update PicturePath if a new file is uploaded
                if (PictureFile != null && PictureFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(PictureFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        PictureFile.CopyTo(fileStream);
                    }

                    // Remove the old picture file if it exists
                    if (!string.IsNullOrEmpty(existingRoom.PicturePath))
                    {
                        var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingRoom.PicturePath.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    existingRoom.PicturePath = "/uploads/" + uniqueFileName;
                }

                // Update amenities
                if (room.Amenities != null)
                {
                    existingRoom.Amenities = room.Amenities.Where(a => !string.IsNullOrWhiteSpace(a)).ToList();
                }

                // Save changes to the database
                _context.Rooms.Update(existingRoom);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(room);
        }

    }
}


       