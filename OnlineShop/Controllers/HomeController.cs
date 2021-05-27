using Model.Dao;
using OnlineShop.Common;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var slides = new SlideDao().ListAll();
            ViewBag.Slides = slides;

            var productDao = new ProductDao();
            ViewBag.ListNewProducts = productDao.ListNewProduct(4);
            ViewBag.ListFeatureProducts = productDao.ListFeatureProduct(4);

            return View();
        }

        //[ChildActionOnly]
        [OutputCache(Duration = 3600 * 24)]
        public ActionResult MainMenu()
        {
            var model = new MenuDao().ListByGroupId(1);
            
            return PartialView(model);
        }

        //[ChildActionOnly]
        [OutputCache(Duration = 3600 * 24)]
        public ActionResult TopMenu()
        {
            var model = new MenuDao().ListByGroupId(2);

            return PartialView(model);
        }
        //[ChildActionOnly]
        [OutputCache(Duration = 3600 * 24)]
        public ActionResult Footer()
        {
            var model = new FooterDao().Footer();

            return PartialView(model);
        }

        //render partial view cho Header Cart
        //[ChildActionOnly]
        public PartialViewResult HeaderCart()
        {
            var cart = Session[CommonConstants.CART_SESSION];
            var list = new List<CartItem>();
            if(cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return PartialView(list);
        }
    }
}