using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class RegisterModel
    {
        [Key]
        public long ID { set; get; }

        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Cần nhập tên đăng nhập")]
        public string UserName { set; get; }

        [Display(Name = "Mật khẩu")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Độ dài mật khẩu ít nhất 6 kí tự")]
        [Required(ErrorMessage = "Cần nhập mật khẩu")]
        public string Password { set; get; }

        [Display(Name = "Nhập lại mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu nhập lại không đúng")]
        public string ConfirmPassword { set; get; }

        [Display(Name = "Tên đầy đủ")]
        [Required(ErrorMessage = "Cần nhập tên đầy đủ")]
        public string Name { set; get; }

        [Display(Name = "Địa chỉ")]
        public string Address { set; get; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Cần nhập email")]
        public string Email { set; get; }

        [Display(Name = "Điện thoại")]
        public string Phone { set; get; }

        [Display(Name = "Tỉnh/Thành")]
        public string ProvinceID { set; get; }

        [Display(Name = "Quận/Huyện")]
        public string DistrictID { set; get; }

        public string CaptchaCode { get; set; }

    }

}