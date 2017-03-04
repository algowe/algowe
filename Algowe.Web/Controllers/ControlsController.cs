using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Algowe.Web.Controllers
{
	public class ControlsController : BaseController
    {
        //
        // GET: /Controls/

        public ActionResult Controls()
        {
            return View();
        }

        public ActionResult Search(string userInput, string resultCount)
        {
            List<AutoItem> lst = new List<AutoItem>();
            lst.Add(new AutoItem() { id = 1, text = userInput + "aaa", value = userInput + "aaa" });
            lst.Add(new AutoItem() { id = 2, text = userInput + "aab", value = userInput + "aab" });
            lst.Add(new AutoItem() { id = 3, text = userInput + "aac", value = userInput + "aac" });
            lst.Add(new AutoItem() { id = 4, text = userInput + "aad", value = userInput + "aad" });
            lst.Add(new AutoItem() { id = 5, text = userInput + "aae", value = userInput + "aae" });

            return Json(lst);
        }


        public ActionResult Tab1()
        {
            /* Set a 3 seconds sleep to show the Tab.Spinning value */
            System.Threading.Thread.Sleep(3000);
            return View();
        }

        public ActionResult Accordion1()
        {
            /* Set a 3 seconds sleep to show the Tab.Spinning value */
            //System.Threading.Thread.Sleep(3000);
            return View();
        }
    }

    public class AutoItem
    {
        public string text { get; set; }
        public string value { get; set; }
        public int id { get; set; }
    }
}
