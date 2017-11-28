using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ExcitedEmu.Models;
using ExcitedEmu.Factories;
namespace ExcitedEmu.Controllers
{
    public class EventController : Controller
    {
        private readonly EventFactory EventFactory;
        public EventController(EventFactory connect)
        {
            EventFactory = connect;
        }
        // Load Objects Page
        [HttpGet]
        [Route("/Home")]
        public IActionResult Home()
        {
            if (HttpContext.Session.GetString("loggedIn")=="true")
            {
                ViewBag.loggedIn = true;
                ViewBag.userID = HttpContext.Session.GetInt32("userID");
                ViewBag.AllEvents = EventFactory.GetEvents();
                ViewBag.JoinedEvents = EventFactory.MyEvents((int)HttpContext.Session.GetInt32("userID"));
                ViewBag.errors = "";
                return View();
            }
            return RedirectToAction("Index","Home");
        }
        [HttpGet]
        [Route("/New")]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("loggedIn")=="true")
            {
                ViewBag.loggedIn = true;
                ViewBag.userID = HttpContext.Session.GetInt32("userID");
                ViewBag.errors = "";
                return View();
            }
            return RedirectToAction("Index","Home");
        }
        [HttpGet]
        [Route("/Event/{EventID}")]
        public IActionResult Show(int EventID)
        {
            if (HttpContext.Session.GetString("loggedIn")=="true")
            {
                ViewBag.loggedIn = true;
                ViewBag.userID = HttpContext.Session.GetInt32("userID");
                ViewBag.errors = "";
                ViewBag.Event = EventFactory.GetEvent(EventID);
                ViewBag.Participants = EventFactory.GetParticipants(EventID);
                ViewBag.activities = EventFactory.GetEActivities(EventID);
                return View();
            }
            return RedirectToAction("Index","Home");
        }
        [HttpPost]
        [Route("/leaveEvent/{EventID}/{userID}")]
        public IActionResult Leave(int EventID, int userID)
        {
            if (HttpContext.Session.GetInt32("userID")==userID)
            {
                EventFactory.LeaveEvent(EventID,userID);
            }     
            return RedirectToAction("Home");
        }
        [HttpPost]
        [Route("/joinEvent/{EventID}/{userID}")]
        public IActionResult Join(int EventID, int userID)
        {
            if (HttpContext.Session.GetInt32("userID")==userID)
            {
                EventFactory.JoinEvent(EventID,userID);
            }     
            return RedirectToAction("Home");
        }
        [HttpGet]
        [Route("/newActivity/{EventID}/{userID}")]
        public IActionResult CreateActivity(int EventID, int userID)
        {
            if (HttpContext.Session.GetString("loggedIn")=="true")
            {
                ViewBag.loggedIn = true;
                ViewBag.userID = userID;
                ViewBag.EventID = EventID;
                ViewBag.errors = "";
                return View();
            }
            return RedirectToAction("Index","Home");
        }
        [HttpPost]
        [Route("/addActivity")]
        public IActionResult NewActivity(Activity Activity)
        {
            if (HttpContext.Session.GetInt32("userID")==Activity.users_idusers)
            {
                EventFactory.NewActivity(Activity);
            }     
            return RedirectToAction("Home");
        }
        [HttpPost]
        [Route("/deleteEvent/{EventID}/{userID}")]
        public IActionResult Delete(int EventID, int userID)
        {
            if (HttpContext.Session.GetInt32("userID")==userID)
            {
                EventFactory.DeleteEvent(EventID);
            }     
            return RedirectToAction("Home");
        }
        // Add New Object
        [HttpPost]
        [Route("/addEvent")]
        public IActionResult AddEvent(Event Event)
        {   
            if (ModelState.IsValid)
            {
                if (HttpContext.Session.GetString("loggedIn")=="true")
                {
                    int IDresult = EventFactory.AddEvent(Event,(int)HttpContext.Session.GetInt32("userID"));                  
                    return RedirectToAction("Show", new {EventID = IDresult});
                }
            }
            ViewBag.errors = ModelState.Values;
            return RedirectToAction("Create");
        }
    }
}
