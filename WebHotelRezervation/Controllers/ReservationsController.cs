using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebHotelRezervation.Models;

namespace WebHotelRezervation.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly HotelDbContext _context;

        public ReservationsController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var hotelDbContext = _context.Reservations.Include(r => r.Client).Include(r => r.Room);
            return View(await hotelDbContext.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name");
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Number");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClientId,RoomId,Date,StayDuration")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                // Проверка дали стаята вече е резервирана
                bool isRoomOccupied = _context.Reservations
                    .Any(r => r.RoomId == reservation.RoomId
                              && r.Date <= reservation.Date
                              && reservation.Date < r.Date.AddDays(r.StayDuration));

                if (isRoomOccupied)
                {
                    // Добавяне на грешка към модела
                    ModelState.AddModelError("RoomId", "The selected room is already reserved for the specified period.");

                    // Презареждане на ViewData за dropdown списъците
                    ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", reservation.ClientId);
                    ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Number", reservation.RoomId);

                    return View(reservation); // Връщане на формата с грешка
                }

                // Ако стаята не е заета, добави резервацията
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Ако ModelState е невалиден, върни формата с dropdown списъците
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", reservation.ClientId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Number", reservation.RoomId);
            return View(reservation);
        }


        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", reservation.ClientId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Number", reservation.RoomId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClientId,RoomId,Date,StayDuration")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", reservation.ClientId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Number", reservation.RoomId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
