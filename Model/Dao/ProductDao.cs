using Model.EF;
using Model.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ProductDao
    {
        OnlineShopDbContext db = null;
        public ProductDao()
        {
            db = new OnlineShopDbContext();
        }
        public List<Product> ListNewProduct(int top)
        {
            return db.Products.Where(x => x.Status == true).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }

        public List<Product> ListFeatureProduct(int top)
        {
            return db.Products.Where(x => x.Status == true && x.TopHot != null && x.TopHot > DateTime.Now).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }
        public Product Detail(long id)
        {
            return db.Products.Find(id);
        }

        public List<Product> RelatedList(long id)
        {
            var product = db.Products.Find(id);
            return db.Products.Where(x => x.ID == id && x.CategoryID == product.CategoryID).ToList();
        }


        public IEnumerable<ProductViewModel> ListByCategoryID(long id, int pageNumber, int size, string searchString)
        {
            var model = from a in db.Products
                        join b in db.ProductCategories
                        on a.CategoryID equals b.ID
                        where a.CategoryID == id
                        select new ProductViewModel()
                        {
                            ID = a.ID,
                            Image = a.Image,
                            Name = a.Name,
                            Detail = a.Detail,
                            Price = a.Price,
                            CateName = b.Name,
                            CateMetaTitle = b.MetaTitle,
                            MetaTitle = a.MetaTitle,
                            CreatedDate = a.CreatedDate
                        };
            var list = model.ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                return model.Where(x => x.Name.Contains(searchString) || x.Detail.Contains(searchString)).OrderByDescending(x => x.CreatedDate).ToPagedList(pageNumber, size);
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(pageNumber, size);
        }

        public IEnumerable<ProductViewModel> Search(int pageNumber, int size, string keyword)
        {
            var model = (from a in db.Products
                         join b in db.ProductCategories
                         on a.CategoryID equals b.ID
                         where a.Name.Contains(keyword)
                         select new 
                         {
                             ID = a.ID,
                             Image = a.Image,
                             Name = a.Name,
                             Detail = a.Detail,
                             Price = a.Price,
                             CateName = b.Name,
                             CateMetaTitle = b.MetaTitle,
                             MetaTitle = a.MetaTitle,
                             CreatedDate = a.CreatedDate
                         }).AsEnumerable().Select(x => new ProductViewModel()
                         {
                             ID = x.ID,
                             Image = x.Image,
                             Name = x.Name,
                             Detail = x.Detail,
                             Price = x.Price,
                             CateName = x.Name,
                             CateMetaTitle = x.MetaTitle,
                             MetaTitle = x.MetaTitle,
                             CreatedDate = x.CreatedDate
                         });

            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(pageNumber, size);
        }

        public List<string> ListName(string keyword)
        {
            return db.Products.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }
    }
}
