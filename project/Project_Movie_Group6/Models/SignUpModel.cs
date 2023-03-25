using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Project_Movie_Group6.Models
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "Tên không được rỗng")]
        [Display(Name = "Fullname")]
        public string Fullname { get; set; }

		[Required(ErrorMessage = "Giới tính không được rỗng")]
		public string Gender { get; set; }
        public string[] Genders = new[] { "Nam", "Nữ", "Khác" }; 

        [Required(ErrorMessage = "Email không được rỗng")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được rỗng")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        //public int? Type { get; set; } //tyoe = 2 is default
        //public bool? IsActive { get; set; } isactive is true is default

    }
}
