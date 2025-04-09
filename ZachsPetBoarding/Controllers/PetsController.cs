using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Data.Entity;
using ZachsPetBoarding.Models;
using ZachsPetBoarding.ViewModels;

namespace ZachsPetBoarding.Controllers
{
    public class PetsController : Controller
    {
        // GET: Pets
        public ActionResult Index()
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
                .Include(x => x.Pet.Medications)
                .Where(x => x.Owner.Id == aspNetUser.Id)
                .ToList();


            return View(ownersToPetsList);
        }

    


        public ActionResult AddPets()
        {
            PetsViewModel petsViewModel = new PetsViewModel();

            return View(petsViewModel);
        }

        [HttpPost]
        public ActionResult AddPets(PetsViewModel petsViewModel, string email)
        {

          

            ApplicationDbContext dbContext = new ApplicationDbContext();

            ApplicationUser aspNetUser = new ApplicationUser();
            aspNetUser = dbContext.Users.FirstOrDefault(x => x.Email == email);

            PetModel petModel = new PetModel();
            petModel.Medications = new List<PetMedicationsModel>();
            petModel.Owners = new List<OwnersToPetsModel>();
            aspNetUser.Pets = new List<OwnersToPetsModel>();


            petModel.PetName = petsViewModel.PetName;
            petModel.PetType = petsViewModel.PetType;
            petModel.PetBreed = petsViewModel.PetBreed;
            petModel.PetAge = petsViewModel.PetAge;
            petModel.FeedingSchedule = petsViewModel.FeedingSchedule;
            petModel.FeedAmountInCups = petsViewModel.FeedAmountInCups;
            
           
            dbContext.Pets.Add(petModel);

           


            foreach (var med in petsViewModel.Medications)
            {
                PetMedicationsModel petMedicationsModel = new PetMedicationsModel();
                petMedicationsModel.MedicationName = med;
                petMedicationsModel.Pet = petModel;

                dbContext.Medications.Add(petMedicationsModel);
                petModel.Medications.Add(petMedicationsModel);

               
            }

            OwnersToPetsModel ownersToPetsModel = new OwnersToPetsModel();
            ownersToPetsModel.Owner = aspNetUser;
            ownersToPetsModel.Pet = petModel;

            dbContext.OwnersToPets.Add(ownersToPetsModel);
            petModel.Owners.Add(ownersToPetsModel);
            aspNetUser.Pets.Add(ownersToPetsModel);
            dbContext.SaveChanges();

            return RedirectToAction("Index", "Pets");
        }

        [HttpPost]
        public ActionResult UpdatePet(Guid PetID, Guid OwnerID, string PetName, string PetType, string PetBreed, string PetAge, string FeedingSchedule, string FeedAmountInCups, List<string> Medications, List<Guid> MedicationIDs, List<string> NewMedications)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();   

            PetModel petModel = dbContext.Pets.FirstOrDefault(x => x.PetID == PetID);
           

            if (petModel != null)
            {
                petModel.PetAge = PetAge;
                petModel.FeedingSchedule = FeedingSchedule;
                petModel.FeedAmountInCups = FeedAmountInCups;
                dbContext.SaveChanges();
            }

            if (MedicationIDs != null && MedicationIDs.Count > 1)
            {
                foreach (var med in MedicationIDs)
                {

                    PetMedicationsModel petMedicationsModel = dbContext.Medications.FirstOrDefault(x => x.MedicationID == med);
                    if (String.IsNullOrEmpty(Medications[MedicationIDs.IndexOf(med)]))
                    {
                        dbContext.Medications.Remove(petMedicationsModel);
                        dbContext.SaveChanges();
                    }
                    petMedicationsModel.MedicationName = Medications[MedicationIDs.IndexOf(med)];
                    dbContext.SaveChanges();
                }
            }
            else if (MedicationIDs != null &&  MedicationIDs.Count == 1)
            {
                Guid MedID = MedicationIDs[0];
                string MedicationName = Medications[0];
                PetMedicationsModel petMedicationsModel = dbContext.Medications.FirstOrDefault(x => x.MedicationID == MedID);
                if (String.IsNullOrEmpty(MedicationName))
                {
                    dbContext.Medications.Remove(petMedicationsModel);
                    dbContext.SaveChanges();
                }
                else
                {
                    petMedicationsModel.MedicationName = Medications[0];
                    dbContext.SaveChanges();
                }
            }

            if  (NewMedications != null)
            {
                foreach (var med in NewMedications)
                {
                    if (!String.IsNullOrEmpty(med))
                    {
                        PetMedicationsModel petMedicationsModel = new PetMedicationsModel();
                        petMedicationsModel.MedicationName = med;
                        petMedicationsModel.Pet = petModel;
                        dbContext.Medications.Add(petMedicationsModel);
                        dbContext.SaveChanges();
                    }
                }
            }

            return RedirectToAction("Index", "Pets");
        }

        // /AddPet?petName=Buddy&petType=Dog&petBreed=Labrador&medications=Heartworm+Prevention
        //public ActionResult AddPet(PetsViewModel petViewModel)
        //{

        //    ApplicationDbContext dbContext = new ApplicationDbContext();

        //    //if (medications == null)
        //    //{
        //    //    return Content("Medications cannot be null, please add a value");
        //    //}

        //    PetModel petModel = new PetModel();
        //    petModel.PetName = petViewModel.PetName;
        //    petModel.PetType = petViewModel.PetType;
        //    petModel.PetBreed = petViewModel.PetBreed;
        //    petModel.PetAge = petViewModel.PetAge;
        //    petModel.FeedingSchedule = petViewModel.FeedingSchedule;
        //    petModel.FeedAmountInCups = petViewModel.FeedAmountInCups;
        //    if (petViewModel.Medications == null)
        //    {
        //        petViewModel.Medications = new List<string>();
        //    }
        //    petModel.Medications = petViewModel.Medications;


        //    dbContext.Pets.Add(petModel);

        //    try
        //    {
        //        dbContext.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return Content($"Added: {petModel.PetName}");
        //}

        //public ActionResult ViewPets(Guid id)
        //{
        //    ApplicationDbContext dbContext = new ApplicationDbContext();

        //   PetModel petsModel = dbContext.Pets.FirstOrDefault(x => x.PetID == id);

        //    if(petsModel != null)
        //    {
        //        return Content($"Pet Name: {petsModel.PetName}, Pet Type: {petsModel.PetType}, Pet Breed: {petsModel.PetBreed}, Medications: {petsModel.Medications}");
        //    }
        //    else
        //    {
        //        return Content("Could not find pet");
        //    }


        //}

        //public ActionResult UpdatePetMedication(Guid? id, string medications)
        //{

        //    if (id == null)
        //    {
        //        return Content("Error: ID null or invalid");
        //    }

        //    ApplicationDbContext dbContext = new ApplicationDbContext();

        //    PetModel petModel = dbContext.Pets.FirstOrDefault(x => x.PetID == id);

        //    if (petModel != null)
        //    {
        //        petModel.Medications = medications;
        //    }

        //    try
        //    {
        //        dbContext.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return Content($"Updated: {petModel.PetName}");
        //}

      
        public ActionResult DeletePet(Guid? id)
        {

            if (id == null)
            {
                return Content("Error: ID null or invalid");
            }

            ApplicationDbContext dbContext = new ApplicationDbContext();

            PetModel petModel = dbContext.Pets.FirstOrDefault(x => x.PetID == id);
            List<PetMedicationsModel> petMedications = dbContext.Medications
                .Where(x => x.Pet.PetID == id)
                .ToList();

            if (petModel != null)
            {
                dbContext.Medications.RemoveRange(petMedications);
                dbContext.Pets.Remove(petModel);
            }

            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

            }

            return RedirectToAction("Index", "Pets");

        }

    }
}