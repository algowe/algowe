using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Algowe.Web.Controllers
{
	public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome!";
			return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
