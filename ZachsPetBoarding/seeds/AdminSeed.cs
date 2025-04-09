using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZachsPetBoarding.Models;

namespace ZachsPetBoarding.seeds
{

  
    public class AdminSeed
    {
        private void CreateInitialAdmin()
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();

            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dbContext));
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(dbContext));

            if (!roleManager.RoleExists("Admin"))
            {
                roleManager.Create(new IdentityRole("Admin"));
            }
            else
            {
                return;
            }

            ApplicationUser user = new ApplicationUser();
            user.UserName = "zpb.admin@zpetboarding.com";
            user.Email = "zpb.admin@zpetboarding.com";
            user.FirstName = "Admin";
            user.LastName = "One";
            user.PhoneNumber = "1234567890";

            string password = "zPetBoarding!2";

            userManager.Create(user, password);
            userManager.AddToRole(user.Id, "Admin");

        }

        public void SeedAdmin()
        {
            CreateInitialAdmin();
        }

    }
}