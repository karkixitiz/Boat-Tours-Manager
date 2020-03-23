using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoatToursManager.Models
{
    public class UserRegistrationModel
    {
        [Required(ErrorMessage = "Please enter name"), MaxLength(30)]
        public string name { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please enter Email ID")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        public string email { get; set; }
        [Required]
        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        //[RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string newPassword { get; set; }
        [Required]
        [StringLength(18, ErrorMessage = "Confirm Password is required", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string confirmPassword { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string phone { get; set; }
        [Required(ErrorMessage = "Please enter address name")]
        public string addressName { get; set; }
        [Required(ErrorMessage = "Please enter street name")]
        public string streetName { get; set; }
        [Required(ErrorMessage = "Please enter location name")]
        public string location { get; set; }
        [Required(ErrorMessage = "Please enter postal code")]
        public string postalCode { get; set; }
        [Required(ErrorMessage = "Please enter country name")]
        public string country { get; set; }
    }
}