using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SPDC.Model.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    [Table("Users")]
    public class ApplicationUser : IdentityUser<int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }
        public override string UserName { get => base.UserName; set => base.UserName = value; }
        public override string Email { get => base.Email; set => base.Email = value; }

        [StringLength(50)]
        [Index("INDEX_CICNUMBER", IsUnique = true)]
        public string CICNumber { get; set; }

        public int? Status { get; set; }

        public int? CommunicationLanguage { get; set; }

        public bool IsNotReceiveInfomation { get; set; }

        [StringLength(256)]
        public string OtherEmail { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? CreateBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public int? LastModifiedBy { get; set; }

        public DateTime? DeleteDate { get; set; }

        public int? DeleteBy { get; set; }
        [StringLength(256)]
        public string DisplayName { get; set; }

        [StringLength(256)]
        public string AdminEmail { get; set; }

        public override ICollection<CustomUserRole> Roles { get => base.Roles; }

        public virtual ICollection<Application> Applications { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
        
        public virtual ICollection<EmployerRecommendation> EmployerRecommendations { get; set; }

        public virtual ICollection<EmployerRecommendationTemp> EmployerRecommendationTemps { get; set; }

        public virtual Language Language { get; set; }

        public virtual ICollection<LessonAttendance> LessonAttendances { get; set; }

        public virtual Particular Particular { get; set; }

        //public virtual ICollection<PaymentTransaction> PaymentTransactions { get; set; }

        public virtual ICollection<Qualification> Qualifications { get; set; }

        public virtual ICollection<QualificationTemp> QualificationTemps { get; set; }
        
        public virtual ICollection<SystemPrivilege> SystemPrivileges { get; set; }

        public virtual ICollection<UserDocument> UserDocuments { get; set; }

        public virtual ICollection<UserDocumentTemp> UserDocumentTemps { get; set; }

        public virtual ICollection<WorkExperience> WorkExperiences { get; set; }

        public virtual ICollection<WorkExperienceTemp> WorkExperienceTemps { get; set; }        

        public virtual AdminPermission AdminPermission { get; set; }

        public virtual ICollection<UserDevice> UserDevices { get; set; }

        public virtual ICollection<NotificationUser> NotificationUsers { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            // Note the authenticationType must match the one defined in
            // CookieAuthenticationOptions.AuthenticationType 
            var userIdentity = await manager.CreateIdentityAsync(
                this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here 
            return userIdentity;
        }
    }


    public class CustomUserRole : IdentityUserRole<int>
    {
        public override int UserId { get => base.UserId; set => base.UserId = value; }

        public override int RoleId { get => base.RoleId; set => base.RoleId = value; }
    }
    public class CustomUserClaim : IdentityUserClaim<int> { }
    public class CustomUserLogin : IdentityUserLogin<int> { }

    public class CustomRole : IdentityRole<int, CustomUserRole>
    {
        public CustomRole() { }
        public CustomRole(string name) { Name = name; }


    }

}