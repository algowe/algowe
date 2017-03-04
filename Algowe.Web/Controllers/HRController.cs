using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Algowe.Web.Models;

namespace Algowe.Web.Controllers
{
    [Authorize(Roles = Global.AConst.RoleSuperAdmin + "," + Global.AConst.RoleAdmin + "," + Global.AConst.RoleUser)]
    public class HRController : CRUDController
    {
        // GET: HR
        public ActionResult Index()
        {
            return View(Repository.Persons);
        }

        // GET: HR/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HR/Create
        public ActionResult Create()
        {
            return View(CreateEmptyModel());
        }

        HrPersonModel CreateEmptyModel()
        {
            return new HrPersonModel() { Person = new Entities.HrPerson(), Departments = Repository.Departments.ToList() };
        }

        // POST: HR/Create
        [HttpPost]
        public ActionResult Create(HrPersonModel Model, FormCollection collection)
        {
            var act = new Action(() =>
            {
                int val = -1;
                if (int.TryParse(collection["SelectedDepartment"], out val) && val >= 0)
                    Model.Person.Department = CreateEmptyModel().Departments.Find(_ => _.Id == val);
                Repository.CreatePerson(Model.Person);
            });

            return Create(act, CreateEmptyModel);
        }

        // GET: HR/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HR/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        ActionResult Del(int id)
        {
            return Delete2(id, () => false, Repository.RemoveUser, "Unable to delete person"); ;
        }

        // GET: HR/Delete/5
        public ActionResult Delete(int id)
        {
            return Del(id);
        }

        // POST: HR/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            return Del(id);
        }
    }
}
