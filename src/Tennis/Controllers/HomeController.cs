using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Tennis.Models;
using Tennis.ViewModels;

namespace Tennis.Controllers
{
    public class HomeController : Controller
    {
        private ModelContext database;

        public HomeController(ModelContext database)
        {
            this.database = database;
        }

        private IEnumerable<Reservation> reservationsForToday()
        {
            var now = DateTime.Now;
            var reservations = from reservation in this.database.Reservations
                               where reservation.Start.Day == now.Day
                               orderby reservation.Start ascending
                               select reservation;
            return reservations;
        }

        private IEnumerable<Reservation> reservations()
        {
            var now = DateTime.Now;
            var reservations = from reservation in this.database.Reservations
                               where reservation.Start.Day >= now.Day
                               orderby reservation.Start ascending
                               select reservation;
            return reservations;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(this.reservationsForToday());
        }

        [HttpGet]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult New([FromForm] ReservationVM reservationVM)
        {
            if (!ModelState.IsValid)
            {
                return View(reservationVM);
            }

            var reservation = new Reservation
            {
                Reserver = reservationVM.Reserver,
                Start = reservationVM.Start,
                End = reservationVM.End
            };

            bool existsConflicting = this.database.Reservations
                .Any(dbReservation =>
                    // Start of new reservation during other reservation
                    (reservation.Start >= dbReservation.Start && reservation.Start <= dbReservation.End) ||
                    // Or end of new reservation during other reservation
                    (reservation.End >= dbReservation.Start && reservation.End <= dbReservation.End)
                );

            if (existsConflicting)
            {
                ViewBag.ErrorMessage = "A reservation conflicts in the chosen range.";
                return View();
            }

            // End cannot occur after start
            if (reservation.End <= reservation.Start)
            {
                ViewBag.ErrorMessage = "End time cannot occur before start time.";
                return View();
            }

            this.database.Reservations.Add(reservation);
            this.database.SaveChanges();
            return RedirectToAction("All");
        }

        [HttpGet]
        public IActionResult All()
        {
            return View(this.reservations());
        }
    }
}
