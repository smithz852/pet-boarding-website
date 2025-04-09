using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZachsPetBoarding.Models;

namespace ZachsPetBoarding.Controllers
{
    public class KennelController : Controller
    {
        // GET: Kennel
        public ActionResult Index()
        {
            return View();
        }

        // /Kennel/CreateKennel
  

        public ActionResult CreateKennel()
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            KennelsModel kennelModel = new KennelsModel();

            int topKennelNumber = dbContext.Kennels.Any() ? dbContext.Kennels.Max(k => k.KennelNumber) : 0;
            kennelModel.KennelNumber = topKennelNumber + 1;
            kennelModel.IsReserved = false;
            kennelModel.Rate = 50;

            dbContext.Kennels.Add(kennelModel);

            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return Content($"Kennel Creation Failed: {ex.Message}");
            }

            return Content("Kennel Created");

        }
    }
}