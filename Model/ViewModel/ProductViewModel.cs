using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class ProductViewModel
    {
        public long ID { set; get; }
        public string Image { set; get; }
        public string Name { set; get; }
        public string Detail { set; get; }
        public decimal? Price { set; get; }
        public string CateName { set; get; }
        public string CateMetaTitle { set; get; }
        public string MetaTitle { get; set; }
        public DateTime? CreatedDate { set; get; }
    }
}
