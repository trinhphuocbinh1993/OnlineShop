﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Cần nhập Username")]
        public string UserName { set; get; }
        [Required(ErrorMessage = "Cần nhập Password")]
        public string Password { set; get; }
        public bool RememberMe { set; get; }
    }
}