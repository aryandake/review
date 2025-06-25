using Fiction2Fact.App_Code;
using Fiction2Fact.Models;
using Fiction2Fact.Models.ViewModels.Advertisement;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using static Fiction2Fact.Models.General;

namespace Fiction2Fact.Controllers.Masters
{
    [Authorize]
    public class AdminMastersController : Libraries.AdminController
    {
        F2FDbContext db = new F2FDbContext();
        // GET: Admin
        public ContentResult Index()
        {
            var menus = (from m in db.Menus
                         join pm in db.ProductMenuMappings on m.MenuId equals pm.MenuId
                         join p in db.Products on pm.ProductId equals p.ProductId
                         select new { m, p }).ToList();
            IEnumerable<MenuModel> lstMenu = db.Menus.ToList();
            StringBuilder sb = new StringBuilder(); ;
            sb.Append(string.Join(",", menus.Select(m=>m.p.ProductName).ToList()));
            return Content(sb.ToString(), "text/plain",UTF8Encoding.UTF8);
        }
        public ActionResult MenuIndex()
        {
            IEnumerable<MenuModel> lstMenu = db.Menus.Include("Parent").OrderBy(m => m.Parent.MenuName).ThenBy(m => m.MenuSortOrder).ToList();
            return View(lstMenu);
        }
        public ActionResult AddEditMenu(int? id)
        {
            MenuModel oMenu = new MenuModel();
            SaveMenuViewModel saveData = new SaveMenuViewModel();
            saveData.Menu = db.Menus.Find(id);
            saveData.ParentMenuListItems = oMenu.FillParentMenuListItems();
            //saveData.ProductListItems = oMenu.FillProductListItems();
            if(saveData.Menu == null)
            {
                saveData.Menu = new MenuModel();
            }
            saveData.Menu.Menus = db.Menus.Where(m=>m.MenuHtml.Equals("#")).ToList();
            //saveData.Menu.Products = db.Products.ToList();
            return View(saveData);
        }
        public ActionResult SaveMenu(MenuModel menu)
        {
            if (menu.MenuId == 0)
            {
                db.Menus.Add(menu);
            }
            else
            {
                db.Entry(menu).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();
            Session.Add("SaveMsgSuccess", "Menu Saved Successfully");
            return RedirectToAction("MenuIndex","AdminMasters");
        }
    }
}