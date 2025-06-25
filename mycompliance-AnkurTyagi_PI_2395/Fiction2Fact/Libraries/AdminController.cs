using Fiction2Fact.App_Code;
using Fiction2Fact.Legacy_App_Code.DAL;
using Fiction2Fact.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using static Fiction2Fact.App_Code.F2FDatabase;
using static Fiction2Fact.Models.General;

namespace Fiction2Fact.Libraries
{
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            F2FDbContext DB = new F2FDbContext();
            var ctx = ((IObjectContextAdapter)DB).ObjectContext;
            if (!User.Identity.IsAuthenticated || Session["CMP_ID"] == null)
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }
            string sUserId = User.Identity.Name;
            
            var pMenuString = F2FParameter("@MenuString", F2FDbType.VarChar, "", ParameterDirection.Output, 4000);

            var retRes = (new UtilitiesDAL()).roleMenuMapping(sUserId, Convert.ToInt16(Session["CMP_ID"].ToString()), Request.RawUrl);
            ViewBag.MenuString = retRes;
        }
    }
    public class getRolesMapping_Result
    {
        public int MM_ID { get; set; }
        public string MM_MENU_NAME { get; set; }
    }
}