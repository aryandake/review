using System.Collections.Generic;
using System.Web.Mvc;
using static Fiction2Fact.Models.General;

namespace Fiction2Fact.Models.ViewModels.Advertisement
{
    public class SaveMenuViewModel
    {
        public MenuModel Menu { get; set; }
        public IEnumerable<ProductModel> Products { get; set; }
        public IEnumerable<SelectListItem> ParentMenuListItems { get; set; }
        public IEnumerable<SelectListItem> ProductListItems { get; set; }
    }
}