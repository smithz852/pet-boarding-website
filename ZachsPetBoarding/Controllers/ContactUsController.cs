using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZachsPetBoarding.Models;
using ZachsPetBoarding.ViewModels;

namespace ZachsPetBoarding.Controllers
{
    public class ContactUsController : Controller
    {
        // GET: ContactUs
        public ActionResult Index()
        {
            FormSubmissionVM formSubmissionVM = new FormSubmissionVM();

            formSubmissionVM.FirstName = string.Empty;
            formSubmissionVM.LastName = string.Empty;
            formSubmissionVM.Email = string.Empty;
            formSubmissionVM.PhoneNumber = string.Empty;
            formSubmissionVM.Question = string.Empty;

            return View(formSubmissionVM);
        }

        [HttpPost]
        public ActionResult Index(FormSubmissionVM formSubmissionVM)
        {
            if (!ModelState.IsValid)
            {
                return View(formSubmissionVM);
            }

            ApplicationDbContext dbContext = new ApplicationDbContext();
           
            FormSubmissionModel formSubmissionModel = new FormSubmissionModel();
            formSubmissionModel.FirstName = formSubmissionVM.FirstName;
            formSubmissionModel.LastName = formSubmissionVM.LastName;
            formSubmissionModel.Email = formSubmissionVM.Email;
            formSubmissionModel.Phone = formSubmissionVM.PhoneNumber;
            formSubmissionModel.Question = formSubmissionVM.Question;

            dbContext.FormSubmissions.Add(formSubmissionModel);

            dbContext.SaveChanges();

            return View("FormSubmissionSuccess");
        }
      
    }
}