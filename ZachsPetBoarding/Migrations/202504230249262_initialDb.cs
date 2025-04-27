namespace ZachsPetBoarding.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookingsModels",
                c => new
                    {
                        BookingID = c.Guid(nullable: false),
                        CheckInDate = c.DateTime(nullable: false),
                        CheckOutDate = c.DateTime(nullable: false),
                        TotalCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BillingCode = c.String(),
                        BookingDateTime = c.DateTime(nullable: false),
                        CheckInLog_CheckInTimeID = c.Guid(),
                        CheckOutLog_CheckOutTimeID = c.Guid(),
                        Kennel_KennelID = c.Guid(nullable: false),
                        Pet_PetID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.BookingID)
                .ForeignKey("dbo.CheckInLogModels", t => t.CheckInLog_CheckInTimeID)
                .ForeignKey("dbo.CheckOutLogModels", t => t.CheckOutLog_CheckOutTimeID)
                .ForeignKey("dbo.KennelsModels", t => t.Kennel_KennelID, cascadeDelete: true)
                .ForeignKey("dbo.PetModels", t => t.Pet_PetID, cascadeDelete: true)
                .Index(t => t.CheckInLog_CheckInTimeID)
                .Index(t => t.CheckOutLog_CheckOutTimeID)
                .Index(t => t.Kennel_KennelID)
                .Index(t => t.Pet_PetID);
            
            CreateTable(
                "dbo.CheckInLogModels",
                c => new
                    {
                        CheckInTimeID = c.Guid(nullable: false),
                        CheckInDateTime = c.DateTime(nullable: false),
                        Employee_EmployeeID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.CheckInTimeID)
                .ForeignKey("dbo.EmployeesModels", t => t.Employee_EmployeeID, cascadeDelete: true)
                .Index(t => t.Employee_EmployeeID);
            
            CreateTable(
                "dbo.EmployeesModels",
                c => new
                    {
                        EmployeeID = c.Guid(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        PhoneNum = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.EmployeeID);
            
            CreateTable(
                "dbo.CheckOutLogModels",
                c => new
                    {
                        CheckOutTimeID = c.Guid(nullable: false),
                        CheckOutDateTime = c.DateTime(nullable: false),
                        Employee_EmployeeID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.CheckOutTimeID)
                .ForeignKey("dbo.EmployeesModels", t => t.Employee_EmployeeID, cascadeDelete: true)
                .Index(t => t.Employee_EmployeeID);
            
            CreateTable(
                "dbo.KennelsModels",
                c => new
                    {
                        KennelID = c.Guid(nullable: false),
                        KennelNumber = c.Int(nullable: false),
                        IsReserved = c.Boolean(nullable: false),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.KennelID);
            
            CreateTable(
                "dbo.PetModels",
                c => new
                    {
                        PetID = c.Guid(nullable: false),
                        PetName = c.String(nullable: false),
                        PetType = c.String(),
                        PetBreed = c.String(),
                        PetAge = c.String(),
                        FeedingSchedule = c.String(),
                        FeedAmountInCups = c.String(),
                    })
                .PrimaryKey(t => t.PetID);
            
            CreateTable(
                "dbo.PetMedicationsModels",
                c => new
                    {
                        MedicationID = c.Guid(nullable: false),
                        MedicationName = c.String(nullable: false),
                        Pet_PetID = c.Guid(),
                    })
                .PrimaryKey(t => t.MedicationID)
                .ForeignKey("dbo.PetModels", t => t.Pet_PetID)
                .Index(t => t.Pet_PetID);
            
            CreateTable(
                "dbo.OwnersToPetsModels",
                c => new
                    {
                        OwnerToPetID = c.Guid(nullable: false),
                        Owner_Id = c.String(nullable: false, maxLength: 128),
                        Pet_PetID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.OwnerToPetID)
                .ForeignKey("dbo.AspNetUsers", t => t.Owner_Id, cascadeDelete: true)
                .ForeignKey("dbo.PetModels", t => t.Pet_PetID, cascadeDelete: true)
                .Index(t => t.Owner_Id)
                .Index(t => t.Pet_PetID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.FormSubmissionModels",
                c => new
                    {
                        FormID = c.Guid(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(nullable: false),
                        Phone = c.String(),
                        Question = c.String(nullable: false),
                        IsClosed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.FormID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.BookingsModels", "Pet_PetID", "dbo.PetModels");
            DropForeignKey("dbo.OwnersToPetsModels", "Pet_PetID", "dbo.PetModels");
            DropForeignKey("dbo.OwnersToPetsModels", "Owner_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.PetMedicationsModels", "Pet_PetID", "dbo.PetModels");
            DropForeignKey("dbo.BookingsModels", "Kennel_KennelID", "dbo.KennelsModels");
            DropForeignKey("dbo.CheckInLogModels", "Employee_EmployeeID", "dbo.EmployeesModels");
            DropForeignKey("dbo.CheckOutLogModels", "Employee_EmployeeID", "dbo.EmployeesModels");
            DropForeignKey("dbo.BookingsModels", "CheckOutLog_CheckOutTimeID", "dbo.CheckOutLogModels");
            DropForeignKey("dbo.BookingsModels", "CheckInLog_CheckInTimeID", "dbo.CheckInLogModels");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.OwnersToPetsModels", new[] { "Pet_PetID" });
            DropIndex("dbo.OwnersToPetsModels", new[] { "Owner_Id" });
            DropIndex("dbo.PetMedicationsModels", new[] { "Pet_PetID" });
            DropIndex("dbo.CheckOutLogModels", new[] { "Employee_EmployeeID" });
            DropIndex("dbo.CheckInLogModels", new[] { "Employee_EmployeeID" });
            DropIndex("dbo.BookingsModels", new[] { "Pet_PetID" });
            DropIndex("dbo.BookingsModels", new[] { "Kennel_KennelID" });
            DropIndex("dbo.BookingsModels", new[] { "CheckOutLog_CheckOutTimeID" });
            DropIndex("dbo.BookingsModels", new[] { "CheckInLog_CheckInTimeID" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.FormSubmissionModels");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.OwnersToPetsModels");
            DropTable("dbo.PetMedicationsModels");
            DropTable("dbo.PetModels");
            DropTable("dbo.KennelsModels");
            DropTable("dbo.CheckOutLogModels");
            DropTable("dbo.EmployeesModels");
            DropTable("dbo.CheckInLogModels");
            DropTable("dbo.BookingsModels");
        }
    }
}
