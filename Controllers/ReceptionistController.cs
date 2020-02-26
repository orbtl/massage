using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using massage.Models;
using Microsoft.AspNetCore.Http;

namespace massage.Controllers
{
    [Route("rec")]
    public class ReceptionistController : Controller
    {
        // Database setup
        public ProjectContext dbContext;
        public ReceptionistController(ProjectContext context)
        {
            dbContext = context;
        }

        // User session to keep track who is logged in!!
        private User UserSession {
            get {return dbContext.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));}
            set {HttpContext.Session.SetInt32("UserId", value.UserId);}
        }
        // Method that checks and Redirects to the correct User's Role Dashboard
        private IActionResult AccessCheck() {
            User ActiveUser = UserSession;
            if(ActiveUser == null) {
                return RedirectToAction("Login", "Login");
            } else if (ActiveUser.Role == 0) {
                ////////// REPLACE WITH A REDIRECT TO DEFAULT DASHBOARD //////////
                return RedirectToAction("Dashboard", "Home");
            } else if (ActiveUser.Role == 1) {
                return RedirectToAction("Dashboard", "Practitioner");
            }
            return null;
        }

        // ROUTES

        // REC DASHBOARD
        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            // Checks User's role and login
            AccessCheck();
            ViewModel vm = new ViewModel();
            vm.CurrentUser = dbContext.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));
            vm.AllUsers = dbContext.Users.ToList();
            vm.AllCustomers = dbContext.Customers.ToList();
            vm.AllInsurances = dbContext.Insurances.ToList();
            vm.AllServices = dbContext.Services.ToList();
            vm.AllTimeslots = dbContext.Timeslots.ToList();
            // List<Reservation> list = Query.AllThisWeeksReservations(dbContext);
            // var weeklyReservations = list;
            return View(vm);
        }

        // ALL RESERVATIONS
        [HttpGet("all_res")]
        public IActionResult AllReservations()
        {
            // Checks User's role and login
            AccessCheck();
            Query.AllReservations(dbContext);
            return View();
        }

        // FORM PAGE??
        [HttpGet("new_res")]
        public IActionResult NewReservation()
        {
            // Checks User's role and login
            AccessCheck();
            ViewModel vm = new ViewModel();
            vm.AllUsers = dbContext.Users.ToList();
            vm.AllCustomers = dbContext.Customers.ToList();
            vm.AllInsurances = dbContext.Insurances.ToList();
            vm.AllServices = dbContext.Services.ToList();
            vm.AllTimeslots = dbContext.Timeslots.ToList();
            return View(vm);
        }

        // SUBMIT: create Reservation
        [HttpPost]
        public IActionResult CreateReservation(ViewModel vm)
        {
            if (ModelState.IsValid)
            {
                Query.CreateReservation(vm.OneReservation, dbContext);
                return RedirectToAction("RDashboard", "Receptionist");
            }
            else
            {
                return View("NewReservation");
            }
        }

        // SUBMIT: cancel res
        [HttpPost]
        public IActionResult CancelReservation(Reservation newReservation)
        {
            Query.DeleteReservation(newReservation.ReservationId, dbContext);
            return RedirectToAction("Dashboard", "Receptionist");
        }

        // VIEW ALL CUSTOMER
        [HttpGet]
        public IActionResult AllCustomers()
        {
            // Checks User's role and login
            AccessCheck();
            return View();
        }

        // NEW CUSTOMER FORM?
        [HttpGet]
        public IActionResult NewCustomer()
        {
            // Checks User's role and login
            AccessCheck();
            ViewModel vm = new ViewModel();
            vm.AllUsers = dbContext.Users.ToList();
            vm.AllCustomers = dbContext.Customers.ToList();
            vm.AllInsurances = dbContext.Insurances.ToList();
            vm.AllServices = dbContext.Services.ToList();
            vm.AllTimeslots = dbContext.Timeslots.ToList();
            return View(vm);
        }

        // SUBMIT: create new customer
        [HttpPost]
        public IActionResult CreateCustomer(ViewModel vm)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(vm.OneCustomer);
                dbContext.SaveChanges();
                return RedirectToAction("RDashboard", "Receptionist");
            }
            else
            {
                return View("NewCustomer");
            }
        }

        // MY PROFILE
        [HttpGet]
        public IActionResult MyProfile()
        {
            // Checks User's role and login
            AccessCheck();
            return View();
        }

        // FORM: edit profile
        [HttpGet]
        public IActionResult EditProfile()
        {
            // Checks User's role and login
            AccessCheck();
            return View();
        }

        [HttpPost]
        public IActionResult UpdatedProfile(User receptionist)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(receptionist);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard", "Receptionist");
            }
            else
            {
                return View("EditProfile");
            }
        }

        [HttpGet]
        public IActionResult OneDayAvailability(DateTime day)
        {
            // Checks User's role and login
            AccessCheck();
            ViewModel vm = new ViewModel();
            vm.AllTimeslots = Query.OneDaysTimeslots(day, dbContext);
            return View("DayViewTimeslots", vm);
        }
        
        [HttpGet]
        public IActionResult TodaysAvailability()
        {
            // Checks User's role and login
            AccessCheck();
            ViewModel vm = new ViewModel();
            vm.AllTimeslots = Query.TodaysTimeslots(dbContext);
            return View("DayViewTimeslots", vm);
        }

        [HttpGet]
        public IActionResult ThisWeeksAvailability()
        {
            // Checks User's role and login
            AccessCheck();
            ViewModel vm = new ViewModel();
            vm.AllTimeslots = Query.ThisWeeksTimeslots(dbContext);
            return View("WeekViewTimeslots", vm);
        }
        [HttpGet]
        public IActionResult ThisMonthsAvailability()
        {
            // Checks User's role and login
            AccessCheck();
            ViewModel vm = new ViewModel();
            vm.AllTimeslots = Query.ThisMonthsTimeslots(dbContext);
            return View("MonthViewTimeslots", vm);
        }
        
        [HttpGet]
        public IActionResult OneWeeksAvailability(DateTime startDay)
        {
            // Checks User's role and login
            AccessCheck();
            ViewModel vm = new ViewModel();
            vm.AllTimeslots = Query.OneWeeksTimeslots(startDay, dbContext);
            return View("WeekViewTimeslots", vm);
        }

        [HttpGet]
        public IActionResult OneMonthsAvailability(DateTime dayInMonth)
        {
            // Checks User's role and login
            AccessCheck();
            ViewModel vm = new ViewModel();
            vm.AllTimeslots = Query.OneMonthsTimeslots(dayInMonth, dbContext);
            return View("MonthViewTimeslots", vm);
        }
    }
}
