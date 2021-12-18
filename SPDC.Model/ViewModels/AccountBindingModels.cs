using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using SPDC.Model.Models;

namespace SPDC.Model.ViewModels
{
    // Models used as parameters to AccountController actions.

    public class AddExternalLoginBindingModel
    {
        [Required]
        [Display(Name = "External access token")]
        public string ExternalAccessToken { get; set; }
    }

    public class ChangePasswordBindingModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "pass_wrong_format", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "new_and_confirm_password_different")]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterBindingModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "pass_wrong_format", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "pass_and_confirm_password_different")]
        public string ConfirmPassword { get; set; }

        public string CICNumber { get; set; }
        [Required]
        public int? CommunicationLanguage { get; set; }
        public string HKID { get; set; }
        public string PassportNo { get; set; }
        public string PassportExpiredDate { get; set; }
        public string SurnameCN { get; set; }
        public string GivenNameCN { get; set; }
        [Required]
        public string SurnameEN { get; set; }
        [Required]
        public string GivenNameEN { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        public bool IsNotReceiveInfomation { get; set; }

    }

    public class RegisterExternalBindingModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class RemoveLoginBindingModel
    {
        [Required]
        [Display(Name = "Login provider")]
        public string LoginProvider { get; set; }

        [Required]
        [Display(Name = "Provider key")]
        public string ProviderKey { get; set; }
    }

    public class SetPasswordBindingModel
    {
        [Required]
        [StringLength(15, ErrorMessage = "pass_wrong_format", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "new_and_confirm_password_different")]
        public string ConfirmPassword { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ForgotLoginEmailViewModel
    {
        public string HKID { get; set; }
        public string PassportNo { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string OtherContactEmail { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "pass_wrong_format", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "new_and_confirm_password_different")]
        public string ConfirmPassword { get; set; }

        public string Token { get; set; }
    }

    public class SystemPrivilegeBindingModel
    {
        [Required]
        [Display(Name = "User ID")]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Course IDs")]
        public int[] CourseIds { get; set; }
    }

    public class AccountInfomationBindingModel
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string CICNo { get; set; }
        public string OtherEmailContact { get; set; }
        [Required]
        public int CommunicationLanguage { get; set; }
        public bool IsNotReceiveInfomation { get; set; }
        public List<int> InterestedCourse { get; set; }
        public string CourseInterestedString { get; set; }
    }
}
