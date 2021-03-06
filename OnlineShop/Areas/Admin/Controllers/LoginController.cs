﻿using Model.Dao;
using OnlineShop.Areas.Admin.Models;
using OnlineShop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();

                var res = dao.Login(model.UserName, Encryptor.EncMD5(model.Password), true);
                if(res == 1)
                {
                    var user = dao.GetById(model.UserName);

                    var userSession = new UserLogin
                    {
                        UserID = user.ID,
                        UserName = user.UserName,
                        GroupID = user.GroupID
                    };

                    var listCredentials = dao.GetListCredential(model.UserName);
                    Session.Add(CommonConstants.SESSION_CREDENTIALS, listCredentials);
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    return RedirectToAction("Index", "Home");
                } else if (res == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại"); 
                     
                } else if (res == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khóa");
                } else if (res == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng");
                }
                else if (res == -3)
                {
                    ModelState.AddModelError("", "Tài khoản của bạn không có quyền đăng nhập");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập thất bại");

                }
            }
            return View("Index");
           
        }

        // Logout
        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            return Redirect("/Admin/Login");
        }
    }
}