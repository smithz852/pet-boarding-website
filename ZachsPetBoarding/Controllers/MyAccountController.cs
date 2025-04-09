using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZachsPetBoarding.Models;
using ZachsPetBoarding.ViewModels;

namespace ZachsPetBoarding.Controllers
{
    public class MyAccountController : Controller
    {
        // GET: MyAccount
        public ActionResult Index()
        {
            return View();
        }

       
    }
}