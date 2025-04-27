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

        public ActionResult CreateBooking()
        {
            BookingsVM bookingsVM = new BookingsVM();

            return View(bookingsVM);
        }

        [HttpPost]
        public ActionResult CreateBooking(BookingsVM bookingsVM)
        {
            string userEmail = User.Identity.GetUserName();

           

            

            ApplicationDbContext dbContext = new ApplicationDbContext();
            KennelsModel kennelsModel = new KennelsModel();
            ApplicationUser aspNetUser = new ApplicationUser();
            aspNetUser = dbContext.Users.FirstOrDefault(x => x.Email == userEmail);
            OwnersToPetsModel ownersToPetsModel = new OwnersToPetsModel();
            BookingsModel bookingsModel = new BookingsModel();

            ownersToPetsModel = dbContext.OwnersToPets
                .Include(x => x.Pet)
                .Where(x => x.Owner.Id == aspNetUser.Id)
                .FirstOrDefault(x => x.Pet.PetName == bookingsVM.PetName);

            kennelsModel = dbContext.Kennels
                .Where(x => x.IsReserved == false)
                .FirstOrDefault();

            if (ownersToPetsModel == null)
            {
                return Content("Error: Pet ID is null or invalid");
            }

            if (kennelsModel == null)
            {
                return Content("Error: Kennel not available");
            }


            
            bookingsModel.CheckInDate = bookingsVM.CheckInDate;
            bookingsModel.CheckOutDate = bookingsVM.CheckOutDate;
            //bookingsModel.TotalCost = totalCost;
            bookingsModel.BookingDateTime = DateTime.UtcNow;
            bookingsModel.Kennel = kennelsModel;
            bookingsModel.Pet = ownersToPetsModel.Pet;

            dbContext.Bookings.Add(bookingsModel);

            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

            }

            return RedirectToAction("BookingSuccess", "Bookings");
        }

        public ActionResult BookingSuccess()
        {
            return View();
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