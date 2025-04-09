using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using ZachsPetBoarding.Models;
using System.Configuration;

namespace ZachsPetBoarding
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            try
            {
                return SendEmailAsync(message.Destination, message.Subject, message.Body);
            }
            catch (Exception ex)
            {
                return Task.CompletedTask;
            }

        }

        public async Task SendEmailAsync(string destination, string subject, string body)
        {
            MailMessage mailMessage = GenerateMailMessage(destination, subject, body);
            SmtpClient client = GetSmtpClient();
            await client.SendMailAsync(mailMessage);

            return;

        }

        public static SmtpClient GetSmtpClient()
        {
            SmtpClient smtpClient = new SmtpClient(EmailServiceCredentials.EmailSMTPUrl);
            smtpClient.Port = int.Parse(EmailServiceCredentials.PortNumber);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(EmailServiceCredentials.EmailFromAddress, EmailServiceCredentials.EmailSMTPPasswordHash);

            return smtpClient;
        }

        public static MailMessage GenerateMailMessage(string destination, string subject, string body)
        {
            MailMessage mailMessage = new MailMessage(new MailAddress(EmailServiceCredentials.EmailFromAddress, EmailServiceCredentials.EmailFromName), new MailAddress(destination));
            mailMessage.Subject = EmailServiceCredentials.EmailAppName + " - " + subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;

            return mailMessage;
        }
    }

    public static class EmailServiceCredentials
    {
        public static string EmailSMTPUrl { get; private set; }
        public static string PortNumber { get; private set; }
        public static string EmailSMTPPasswordHash { get; private set; }
        public static string EmailFromAddress { get; private set; }
        public static string EmailFromName { get; private set; }
        public static string EmailAppName { get; private set; }

        public static void SetCredentials(string emailSMTPUrl, string portNumber, string emailSMTPPasswordHash, string emailFromAddress, string emailFromName, string emailAppName)
        {
            EmailSMTPUrl = emailSMTPUrl;
            PortNumber = portNumber;
            EmailSMTPPasswordHash = emailSMTPPasswordHash;
            EmailFromAddress = emailFromAddress;
            EmailFromName = emailFromName;
            EmailAppName = emailAppName;
        }

        //Call from global application
        public static void PopulateEmailCredentialsFromAppConfig()
        {
            string emailSMTPURL = ConfigurationManager.AppSettings["emailSMTPURL"].ToString();
            string portNumber = ConfigurationManager.AppSettings["portNumber"].ToString();
            string emailSMTPPasswordHash = ConfigurationManager.AppSettings["emailSMTPPasswordHash"].ToString();
            string emailFromAddress = ConfigurationManager.AppSettings["emailFromAddress"].ToString();
            string emailFromName = ConfigurationManager.AppSettings["emailFromName"].ToString();
            string emailAppName = ConfigurationManager.AppSettings["emailAppName"].ToString();

            SetCredentials(emailSMTPURL, portNumber, emailSMTPPasswordHash, emailFromAddress, emailFromName, emailAppName);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));

            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
