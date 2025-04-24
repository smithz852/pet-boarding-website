using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ZachsPetBoarding.Models;
using Newtonsoft.Json.Linq;
using ZachsPetBoarding.ViewModels;
using Microsoft.AspNet.Identity;

namespace ZachsPetBoarding.Controllers
{
     
    public class BookingsController : Controller
    {
       
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PetsList()
        {
            string userEmail = User.Identity.GetUserName();

            if (userEmail == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ApplicationDbContext dbContext = new ApplicationDbContext();
            ApplicationUser aspNetUser = new ApplicationUser();
            aspNetUser = dbContext.Users.FirstOrDefault(x => x.Email == userEmail);


            List<OwnersToPetsModel> ownersToPetsList = dbContext.OwnersToPets
                .Include(x => x.Pet)
                .Where(x => x.Owner.Id == aspNetUser.Id)
                .ToList();


            return PartialView("PetsList", ownersToPetsList);
        }

        public ActionResult CreateBooking()
        {
            BookingsVM bookingsVM = new BookingsVM();

            return View(bookingsVM);
        }

        [HttpPost]
        public ActionResult CreateBooking(BookingsVM bookingsVM)
        {
            string userEmail = User.Identity.GetUserName();

           

            //if (kennelID == null)
            //   {
            //       return Content("Error: Kennel ID is null or invalid");
            //   }

            ApplicationDbContext dbContext = new ApplicationDbContext();
            ApplicationUser aspNetUser = new ApplicationUser();
            aspNetUser = dbContext.Users.FirstOrDefault(x => x.Email == userEmail);
            OwnersToPetsModel ownersToPetsModel = new OwnersToPetsModel();
            ownersToPetsModel = dbContext.OwnersToPets
                .Include(x => x.Pet)
                .Where(x => x.Owner.Id == aspNetUser.Id)
                .FirstOrDefault(x => x.Pet.PetName == bookingsVM.PetName);

            if (ownersToPetsModel == null)
            {
                return Content("Error: Pet ID is null or invalid");
            }

            //PetModel petModel = new PetModel();
            //KennelsModel kennelsModel = new KennelsModel();


            //petModel = dbContext.Pets.FirstOrDefault(x => x.PetID == petID);
            //kennelsModel = dbContext.Kennels.FirstOrDefault(x => x.KennelID == kennelID);

            //if (kennelsModel == null)
            //{
            //    return Content("Error: Kennel not found");
            //}

            //if (petModel == null)
            //{
            //    return Content("Error: Pet not found");
            //}


            //BookingsModel bookingsModel = new BookingsModel();
            //bookingsModel.CheckInDate = DateTime.Parse(checkInDate);
            //bookingsModel.CheckOutDate = DateTime.Parse(checkOutDate);
            //bookingsModel.TotalCost = totalCost;
            //bookingsModel.BookingDateTime = DateTime.UtcNow;
            //bookingsModel.Kennel = kennelsModel;
            //bookingsModel.Pet = petModel;

            //dbContext.Bookings.Add(bookingsModel);

            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

            }

            return Content("Booking was successful!");
        }

        public ActionResult ViewBooking(Guid? id)
        {

            if (id == null)
            {
                return Content("Error: Booking ID is null or invalid");
            }

            ApplicationDbContext dbContext = new ApplicationDbContext();

            BookingsModel bookingsModel = dbContext.Bookings
                .Include(x => x.Pet)
                .Include(b => b.Kennel)
                .FirstOrDefault(x => x.BookingID == id);

            if (bookingsModel != null)
            {
                return Content($"Pet ID: {bookingsModel.Pet.PetName}, Kennel ID: {bookingsModel.Kennel.KennelNumber}, Check In Date: {bookingsModel.CheckInDate}" +
                    $", Check Out Date: {bookingsModel.CheckOutDate}" +
                    $", Total Cost: {bookingsModel.TotalCost}" +
                    $", Booking Date Time: {bookingsModel.BookingDateTime}");
            }
            else
            {
                return Content("Booking information not found");
            }


        }

        public ActionResult UpdateBookingKennel(Guid? id, Guid? newKennelID)
        {

            if (id == null)
            {
                return Content("Error: Booking ID is null or invalid");
            }

            if (newKennelID == null)
            {
                return Content("Error: Kennel ID is null or invalid");
            }

            ApplicationDbContext dbContext = new ApplicationDbContext();

            BookingsModel bookingsModel = dbContext.Bookings
                .Include(x => x.Pet)
                .Include(x => x.Kennel)
                .FirstOrDefault(x => x.BookingID == id);
           
            KennelsModel kennelsModel = dbContext.Kennels.FirstOrDefault(x => x.KennelID == newKennelID);

            if (bookingsModel != null && kennelsModel != null && kennelsModel.IsReserved == false)
            {

                bookingsModel.Kennel = kennelsModel; 
               
            }
            else
            {
                return Content("Error updating booking");
            }

            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

            return Content("Updated booking!");

        }



        public ActionResult DeleteBooking(Guid? id)
        {

            if (id == null)
            {
                return Content("Error: Booking ID is null or invalid");
            }

            ApplicationDbContext dbContext = new ApplicationDbContext();

            BookingsModel bookingsModel = dbContext.Bookings.FirstOrDefault(x => x.BookingID == id);

            if (bookingsModel != null)
            {
                dbContext.Bookings.Remove(bookingsModel);
            }

            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

            }

            return Content($"Deleted Booking ID: {id}");

        }

    }
}