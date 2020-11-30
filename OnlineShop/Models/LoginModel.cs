using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class LoginModel
    {
        [Key]
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Cần nhập tên đăng nhập")]
        public string UserName { set; get; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Cần nhập mật khẩu")]
        public string Password { set; get; }
    }
}