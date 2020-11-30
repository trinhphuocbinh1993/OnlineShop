using BotDetect.Web.Mvc;
using Model.Dao;
using Model.EF;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShop.Common;
using Facebook;
using System.Configuration;
using System.Xml.Linq;

namespace OnlineShop.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();

                var res = dao.Login(model.UserName, Encryptor.EncMD5(model.Password));
                if (res == 1)
                {
                    var user = dao.GetById(model.UserName);

                    var userSession = new UserLogin
                    {
                        UserID = user.ID,
                        UserName = user.UserName
                    };

                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    return Redirect("/");
                }
                else if (res == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại");

                }
                else if (res == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khóa");
                }
                else if (res == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập thất bại");

                }
            }
            return View(model);

        }

        [HttpPost]
        public JsonResult LoadProvince()
        {
            var xmlDoc = XDocument.Load(Server.MapPath(@"~/Assets/client/data/Provinces_Data.xml"));

            var xElements = xmlDoc.Element("Root").Elements("Item").Where(x => x.Attribute("type").Value == "province"); ;
            var list = new List<ProvinceModel>();
            ProvinceModel province = null;
            foreach (var item in xElements)
            {
                province = new ProvinceModel();
                province.ID = Convert.ToInt32(item.Attribute("id").Value);
                province.Name = Convert.ToString(item.Attribute("value").Value);
                list.Add(province);
            }
            return Json(new
            {
                status = true,
                data = list
            });
        }

        [HttpPost]
        public JsonResult LoadDistrict()
        {
            int provinceID = Convert.ToInt32(Request.Params["id"]);
            var xmlDoc = XDocument.Load(Server.MapPath(@"~/Assets/client/data/Provinces_Data.xml"));

            var provinceElements = xmlDoc.Element("Root").Elements("Item").Where(x => x.Attribute("type").Value == "province" && Convert.ToInt32(x.Attribute("id").Value) == provinceID); ;
            var districtElements = provinceElements.Elements("Item").Where(x => x.Attribute("type").Value == "district");
            var list = new List<DistrictModel>();
            DistrictModel district = null;
            foreach (var item in districtElements)
            {
                district = new DistrictModel();
                district.ID = Convert.ToInt32(item.Attribute("id").Value);
                district.ProvinceID = provinceID;
                district.Name = Convert.ToString(item.Attribute("value").Value);
                list.Add(district);
            }
            return Json(new
            {
                status = true,
                data = list
            });
        }
        //facebook login
        //private Uri RedirectUri
        //{
        //    get
        //    {
        //        var uriBuilder = new UriBuilder(Request.Url);
        //        uriBuilder.Query = null;
        //        uriBuilder.Fragment = null;
        //        uriBuilder.Path = Url.Action("FacebookCallback");
        //        return uriBuilder.Uri;
        //    }
        //}
        //public ActionResult LoginFacebook()
        //{
        //    var fb = new FacebookClient();
        //    var loginUrl = fb.GetLoginUrl(
        //        new
        //        {
        //            client_id = ConfigurationManager.AppSettings["FbAppId"],
        //            client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
        //            redirect_uri = RedirectUri.AbsoluteUri,
        //            response_type = "code",
        //            scope = "email",
        //        });
        //    return Redirect(loginUrl.AbsoluteUri);
        //}
        //public ActionResult FacebookCallback(string code)
        //{
        //    var fb = new FacebookClient();
        //    dynamic result = fb.Post("https://graph.facebook.com/oath/access_token", new
        //    {
        //        client_id = ConfigurationManager.AppSettings["FbAppId"],
        //        client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
        //        redirect_uri = RedirectUri.AbsoluteUri,
        //        code
        //    });
        //    var accessToken = result.access_token;
        //    if (!string.IsNullOrEmpty(accessToken))
        //    {
        //        fb.AccessToken = accessToken;
        //        dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
        //        string email = me.email;
        //        string userName = me.email;
        //        string firstname = me.first_name;
        //        string middlename = me.middle_name;
        //        string lastname = me.last_name;
        //        var user = new User();
        //        user.Email = email;
        //        user.UserName = userName;
        //        user.Status = true;
        //        user.Name = firstname + " " + middlename + " " + lastname;
        //        user.CreatedDate = DateTime.Now;
        //        var resultInsert = new UserDao().InsertForFacebook(user);
        //        if (resultInsert > 0)
        //        {
        //            var userSession = new UserLogin();
        //            userSession.UserName = user.UserName;
        //            userSession.UserID = user.ID;
        //            Session.Add(CommonConstants.USER_SESSION, userSession);

        //        }
        //    }
        //    return Redirect("/");
        //}
        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            return Redirect("/");
        }
        // POST: User
        [HttpPost]
        [CaptchaValidationActionFilter("CaptchaCode", "ExampleCaptcha", "Mã Captcha không đúng!")]

        public ActionResult Register(OnlineShop.Models.RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (dao.CheckUserName(model.UserName))
                {
                    ModelState.AddModelError("", "Tên đăng nhâp đã tồn tại");
                }
                else if (dao.CheckEmail(model.Email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                }
                else
                {
                    var user = new User();
                    user.UserName = model.UserName;
                    user.Password = Encryptor.EncMD5(model.Password);
                    user.Name = model.Name;
                    user.Address = model.Address;
                    user.Email = model.Email;
                    user.Phone = model.Phone;
                    user.CreatedDate = DateTime.Now;
                    user.Status = true;
                    if (!string.IsNullOrEmpty(model.ProvinceID))
                    {
                        user.ProvinceID = Convert.ToInt32(model.ProvinceID);
                    }
                    if (!string.IsNullOrEmpty(model.DistrictID))
                    {
                        user.DistrictID = Convert.ToInt32(model.DistrictID);
                    }
                    var res = dao.Insert(user);
                    if (res > 0)
                    {
                        ViewBag.Success = "Đăng ký thành công";
                        model = new RegisterModel();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng ký không thành công");
                    }
                }
            }
            return View(model);
        }

    }
}