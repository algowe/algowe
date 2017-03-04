using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Algowe.Web.Global.Auth;
using Algowe.Web.Models;
using Algowe.Web.Entities;

namespace Algowe.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        public IRepository Repository { get; set; }

        /*  public IMapper ModelMapper { get; set; }*/

        public IAuthentication Auth { get; set; }

        public GlUser CurrentUser
        {
            get
            {
                return ((IUserProvider)Auth.CurrentUser.Identity).User;
            }
        }
    }

    public abstract class CRUDController : BaseController
    {
        protected ActionResult Create<T>(Action CreateAct, Func<T> CreateEmptyModelAct)
        {
            try
            {
                CreateAct();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag["Exception"] = ex.Message;
                var m = CreateEmptyModelAct();
                return View(m);
            }
        }

        public ActionResult Delete2(int id, Func<bool> NotDelIfPredicateIsTrue, Func<int, bool> RemoveAction, string UnableDeleteMsg)
        {
            try
            {
                if (NotDelIfPredicateIsTrue())
                {
                    ViewBag["Error"] = UnableDeleteMsg;
                    return RedirectToAction("Index");
                }
                if (RemoveAction(id))
                    return RedirectToAction("Index");
                else
                {
                    ViewBag["Error"] = UnableDeleteMsg;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag["Exception"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
