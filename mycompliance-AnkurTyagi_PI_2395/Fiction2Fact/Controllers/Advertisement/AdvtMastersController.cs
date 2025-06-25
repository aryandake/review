using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Fiction2Fact.Models.Advertisement.AdvertisementMasters;

namespace Fiction2Fact.Controllers.Advertisement
{
    public class AdvtMastersController : Controller
    {
        public AdvtMastersController()
        {
            ViewBag.MenuString = "Test";
        }
        // GET: AdvtMasters
        public ActionResult Categories()
        {
            List<CategoryModel> Categories = new List<CategoryModel>().ToList();
            return View(Categories);
        }
    }
}