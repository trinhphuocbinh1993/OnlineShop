using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        // GET: Admin/Category
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Create(Category model)
        {
            if(ModelState.IsValid)
            {
                var currentCulture = Session[OnlineShop.Common.CommonConstants.CurrentCulture];
                model.Language = currentCulture.ToString();
                var res = new CategoryDao().Insert(model);
                if(res > 0)
                {
                    return RedirectToAction("Index");
                } else
                {
                    ModelState.AddModelError("",StaticResource.Resource.Category_InsertFailed);
                }
            }
            return View(model);
        }

    }
}