using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventBookingSystem.Data;
using EventBookingSystem.Models;

namespace EventBookingSystem.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================
        // READ ALL EVENTS
        // =========================
        public async Task<IActionResult> Index()
        {
            var events = await _context.Events.ToListAsync();
            var bookings = await _context.Bookings.ToListAsync();

            ViewBag.Bookings = bookings;

            return View(events);
        }

        // =========================
        // DETAILS (single event)
        // =========================
        public async Task<IActionResult> Details(int id)
        {
            var eventItem = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventItem == null)
                return NotFound();

            return View(eventItem);
        }

        // Book Event action
        [HttpPost]
        public async Task<IActionResult> Book(int id)
        {
            string userId = "guest"; // later we replace with real login

            // CHECK FOR DUPLICATE
            var alreadyBooked = await _context.Bookings
                .AnyAsync(b => b.EventId == id && b.UserId == userId);

            if (alreadyBooked)
            {
                TempData["Message"] = "You already booked this event!";
                return RedirectToAction("Index");
            }

            // CREATE BOOKING
            var booking = new Booking
            {
                EventId = id,
                UserId = userId,
                BookingDate = DateTime.Now
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Booking successful!";
            return RedirectToAction("Index");
        }

        //Create My bookings page
        public async Task<IActionResult> MyBookings()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Event)
                .ToListAsync();

            return View(bookings);
        }

        // Delete booking action
        [HttpPost]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Booking cancelled successfully!";
            }

            return RedirectToAction("MyBookings");
        }

        // =========================
        // CREATE (GET)
        // =========================
        public IActionResult Create()
        {
            return View();
        }

        // =========================
        // CREATE (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event newEvent)
        {
            if (ModelState.IsValid)
            {
                _context.Events.Add(newEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(newEvent);
        }

        // =========================
        // EDIT (GET)
        // =========================
        public async Task<IActionResult> Edit(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
                return NotFound();

            return View(eventItem);
        }

        // =========================
        // EDIT (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Event updatedEvent)
        {
            if (ModelState.IsValid)
            {
                _context.Events.Update(updatedEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(updatedEvent);
        }

        // =========================
        // DELETE (GET confirm page)
        // =========================
        public async Task<IActionResult> Delete(int id)
        {
            var eventItem = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventItem == null)
                return NotFound();

            return View(eventItem);
        }

        // =========================
        // DELETE (POST actual delete)
        // =========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem != null)
            {
                _context.Events.Remove(eventItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}