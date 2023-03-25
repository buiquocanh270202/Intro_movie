using Microsoft.Build.Framework;
//using Xunit;
//using Xunit.Sdk;

//using Xunit.Abstractions;

namespace Project_Movie_Group6.Models
{
    using System.ComponentModel.DataAnnotations;
    public class LoginModel
    {
        [Required(ErrorMessage ="Email không được rỗng")]
        [Display(Name ="Email")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Mật khẩu không được rỗng")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        
    }
}
