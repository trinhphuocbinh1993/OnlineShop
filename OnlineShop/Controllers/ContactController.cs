using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            var model = new ContactDao().GetActiveContact();
            return View(model);
        }

        [HttpPost]
        public JsonResult Send(string name, string phone, string address, string email, string content)
        {
            var feedback = new Feedback
            {
                Name = name,
                Phone = phone,
                Address = address,
                Email = email,
                Content = content,
                CreatedDate = DateTime.Now
            };

            var id = new ContactDao().InsertFeedBack(feedback);
            if (id > 0)
            {
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }

        }
    }
}