using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZachsPetBoarding.Models;

namespace ZachsPetBoarding.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Bookings()
        {
            ViewBag.Message = "Reservations Page.";

            return View();
        }

        public ActionResult Hours()
        {
            ViewBag.Message = "Hours Page.";

            return View();
        }

       
    }
}