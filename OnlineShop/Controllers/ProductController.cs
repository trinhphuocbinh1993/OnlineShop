using Model.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly] //này chỉ dành cho Partial View gọi thôi
        public PartialViewResult ProductCategory()
        {
            var model = new ProductCategoryDao().ListAll();
            return PartialView(model);
        }

        [HttpGet]
        public ActionResult Category(long id, int? size, int? page,string searchString)
        {
            var model = new ProductCategoryDao().ViewDetail(id);

            // 1. Tham số int? dùng để thể hiện null và kiểu int
            // page có thể có giá trị là null và kiểu int.
            // 1. Tạo list pageSize để người dùng có thể chọn xem để phân trang
            // Bạn có thể thêm bớt tùy ý --- dammio.com
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "5", Value = "5" });
            items.Add(new SelectListItem { Text = "10", Value = "10" });
            items.Add(new SelectListItem { Text = "20", Value = "20" });
            items.Add(new SelectListItem { Text = "25", Value = "25" });
            items.Add(new SelectListItem { Text = "50", Value = "50" });
            items.Add(new SelectListItem { Text = "100", Value = "100" });
            items.Add(new SelectListItem { Text = "200", Value = "200" });
            // 1.1. Giữ trạng thái kích thước trang được chọn trên DropDownList
            foreach (var item in items)
            {
                if (item.Value == size.ToString()) item.Selected = true;
            }
            // 1.2. Tạo các biến ViewBag
            ViewBag.size = items; // ViewBag DropDownList
            ViewBag.currentSize = size; // tạo biến kích thước trang hiện tại

            // 2. Nếu page = null thì đặt lại là 1.
            if (page == null) {
                page = 1;
                   };

            //// 3. Tạo truy vấn, lưu ý phải sắp xếp theo trường nào đó, ví dụ OrderBy
            //// theo LinkID mới có thể phân trang.
            //var links = (from l in db.Links
            //             select l).OrderBy(x => x.LinkID);

            // 4. Tạo kích thước trang (pageSize), mặc định là 5.
            int pageSize = (size ?? 5);
            // 4.1 Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);

            ViewBag.Products = new ProductDao().ListByCategoryID(id, pageNumber, pageSize, searchString);
            ViewBag.SearchString = searchString;
            return View(model);
        }
        [OutputCache(CacheProfile = "Cache1DayForProduct")]
        public ActionResult Detail(long id)
        {
            var model = new ProductDao().Detail(id);
            ViewBag.Category = new ProductCategoryDao().ViewDetail(model.CategoryID.Value);
            ViewBag.RelatedList = new ProductDao().RelatedList(id);
            return View(model);
        }
        
    }
}