using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BoatToursManager.Models
{
    public class UserChangePasswordModel
    {
        [Required]
        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
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
    }
}