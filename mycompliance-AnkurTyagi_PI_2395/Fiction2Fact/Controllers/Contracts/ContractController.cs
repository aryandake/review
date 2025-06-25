using Fiction2Fact.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fiction2Fact.Controllers.Contracts
{
    [Authorize]
    public class ContractController : AdminController
    {
        // GET: Contract
        public ActionResult Index()
        {
            return Content("Test"); ;
        }
    }
}