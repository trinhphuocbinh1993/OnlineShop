using Common;
using Model.Dao;
using Model.EF;
using OnlineShop.Common;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class CartController : Controller
    {

        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session[CommonConstants.CART_SESSION];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }

        public JsonResult Delete(string id)
        {
            var sessionCart = (List<CartItem>)Session[CommonConstants.CART_SESSION];
            sessionCart.RemoveAll(x => x.Product.ID == Convert.ToInt32(id));
            Session[CommonConstants.CART_SESSION] = sessionCart;
            return Json(new
            {
                status = true
            });
        }

        public JsonResult DeleteAll()
        {
            Session[CommonConstants.CART_SESSION] = null;
            return Json(new
            {
                status = true
            });
        }
        public JsonResult Update(string cartModel)
        {
            var cart = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sesssionCart = (List<CartItem>)Session[CommonConstants.CART_SESSION];

            foreach (var item in sesssionCart)
            {
                var jsonItem = cart.SingleOrDefault(x => x.Product.ID == item.Product.ID);
                if (jsonItem != null)
                {
                    item.Quantity = jsonItem.Quantity;
                }
            }
            Session[CommonConstants.CART_SESSION] = sesssionCart;
            return Json(new
            {
                status = true
            });
        }
        public ActionResult AddItem(long productId, int quantity)
        {
            var product = new ProductDao().Detail(productId);
            var cart = Session[CommonConstants.CART_SESSION];
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.Product.ID == productId))
                {
                    foreach (var item in list)
                    {
                        if (item.Product.ID == productId)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    // tạo mới đối tượng cart item
                    var item = new CartItem
                    {
                        Product = product,
                        Quantity = quantity
                    };

                    list.Add(item);
                }
                //Gán vào session
                Session[CommonConstants.CART_SESSION] = list;

            }
            else
            {
                // tạo mới đối tượng cart item
                var item = new CartItem();
                item.Product = product;
                item.Quantity = quantity;
                var list = new List<CartItem>();
                list.Add(item);
                //gán vào session
                Session[CommonConstants.CART_SESSION] = list;
            }

            return RedirectToAction("Index");
        }

        // cho phần thanh toán
        [HttpGet]
        public ActionResult Payment()
        {
            var cart = Session[CommonConstants.CART_SESSION];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }
        [HttpPost]
        public ActionResult Payment(string shipName, string mobile, string address, string email)
        {
            var order = new Order();
            order.CreatedDate = DateTime.Now;
            order.ShipName = shipName;
            order.ShipMobile = mobile;
            order.ShipAddress = address;
            order.ShipEmail = email;

            try
            {
                var id = new OrderDao().Insert(order);
                var cart = (List<CartItem>)Session[CommonConstants.CART_SESSION];
                var detailDao = new OrderDetailDao();
                decimal total = 0;
                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail();
                    orderDetail.ProductID = item.Product.ID;
                    orderDetail.OrderID = id;
                    orderDetail.Price = item.Product.Price;
                    orderDetail.Quantity = item.Quantity;
                    detailDao.Insert(orderDetail);

                    total += item.Product.Price.GetValueOrDefault(0) * item.Quantity;
                }

                string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/client/template/neworder.html"));
                content = content.Replace("{{CustomerName}}", shipName);
                content = content.Replace("{{Phone}}", mobile);
                content = content.Replace("{{Email}}", email);
                content = content.Replace("{{Address}}", address);
                content = content.Replace("{{Total}}", total.ToString("N0"));
                //var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

                new MailHelper().SendMail(email, "Đơn hàng mới từ Dụng Cụ Chợ Lớn", content);
                //new MailHelper().SendMail(toEmail, "Đơn hàng mới từ OnlineShop", content);

            } catch
            {
                return Redirect("/loi-thanh-toan");

            }
            return Redirect("/hoan-thanh");
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}