using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ZachsPetBoarding.Models
{
    //DB context in it's own file
    

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<FormSubmissionModel> FormSubmissions { get; set; }


        public DbSet<PetModel> Pets { get; set; }

        public DbSet<EmployeesModel> Employees { get; set; }

        public DbSet<CheckInLogModel> CheckInLog { get; set; }

        public DbSet<CheckOutLogModel> CheckOutLog { get; set; }

        public DbSet<KennelsModel> Kennels { get; set; }

        public DbSet<BookingsModel> Bookings { get; set; }

        public DbSet<OwnersToPetsModel> OwnersToPets { get; set; }
        public DbSet<PetMedicationsModel> Medications { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}