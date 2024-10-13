using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Warranty.Common.BussinessEntities
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please enter username")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter password")]
        public string UserPassword { get; set; }
        [Required(ErrorMessage = "Please enter captcha")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "Please enter valid captcha")]
        public string CaptchaCode { get; set; }
        public string CaptchaImage { get; set; }

        public string Emailid { get; set; }
        [Required(ErrorMessage = "Please enter password")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password minimum 8 characters")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Please enter confirm password")]
        [Compare("NewPassword", ErrorMessage = "Password and Confirm Password must be same")]
        public string ConfirmPassword { get; set; }

        public string SessionId { get; set; }
        public string UserMasterId { get; set; }
        [Required(ErrorMessage = "Please enter OTP"), MinLength(6, ErrorMessage = "OTP must be 6 digit number"), MaxLength(6, ErrorMessage = "OTP must be 6 digit number")]
        public string OTP { get; set; }

    }
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Please enter username")]
        [Display(Name = "Username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Please enter captcha")]
        [Display(Name = "Captcha")]
        public string CaptchaCode { get; set; }
        public string CaptchaImage { get; set; }
        public string Emailid { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

    }

    public class ResetPasswordModel
    {
        public int UserMasterId { get; set; }
        public string EncId { get; set; }
        public string Username { get; set; }
        public string OldPassword { get; set; }
        [Display(Name = "Password")]
        [StringLength(500, MinimumLength = 8, ErrorMessage = "Password length should be minimum 8 and maximum 500")]
        [Required(ErrorMessage = "Please enter password")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [StringLength(500, MinimumLength = 8, ErrorMessage = "Password length should be minimum 8 and maximum 500")]
        [Required(ErrorMessage = "Please enter confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Confirm password does not match with password.")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Captcha")]
        [Required(ErrorMessage = "Please enter captcha")]
        [MaxLength(4)]
        public string CaptchaCode { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string CaptchaImage { get; set; }
    }
}
