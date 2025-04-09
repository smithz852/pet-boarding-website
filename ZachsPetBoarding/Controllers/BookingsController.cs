using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ZachsPetBoarding.Models;
using Newtonsoft.Json.Linq;

namespace ZachsPetBoarding.Controllers
{
     
    public class BookingsController : Controller
    {
       
        public ActionResult Index()
        {
            return View();
        }

        // Bookings/CreateBooking?checkInDate=3/15/2025&checkOutDate=3/20/2025&totalCost=300&petID=5FE26113-C18A-4226-BD64-1C269CBF2672&kennelID=8E194BCE-9ABF-40CE-8AE0-34DA3308FB10
        public ActionResult CreateBooking(
            string checkInDate, 
            string checkOutDate, 
            decimal totalCost, 
            Guid? petID, 
            Guid? kennelID
            )

        {

         if (petID == null)
            {
                return Content("Error: Pet ID is null or invalid");
            }

         if (kennelID == null)
            {
                return Content("Error: Kennel ID is null or invalid");
            }

            ApplicationDbContext dbContext = new ApplicationDbContext();

            PetModel petModel = new PetModel();
            KennelsModel kennelsModel = new KennelsModel();


            petModel = dbContext.Pets.FirstOrDefault(x => x.PetID == petID);
            kennelsModel = dbContext.Kennels.FirstOrDefault(x => x.KennelID == kennelID);

            if (kennelsModel == null)
            {
                return Content("Error: Kennel not found");
            }

            if (petModel == null)
            {
                return Content("Error: Pet not found");
            }
            

            BookingsModel bookingsModel = new BookingsModel();
            bookingsModel.CheckInDate = DateTime.Parse(checkInDate);
            bookingsModel.CheckOutDate = DateTime.Parse(checkOutDate);
            bookingsModel.TotalCost = totalCost;
            bookingsModel.BookingDateTime = DateTime.UtcNow;
            bookingsModel.Kennel = kennelsModel;
            bookingsModel.Pet = petModel;

            dbContext.Bookings.Add(bookingsModel);

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