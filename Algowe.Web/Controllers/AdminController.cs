using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Algowe.Web.Controllers
{
    [Authorize(Roles = Global.AConst.RoleSuperAdmin +"," + Global.AConst.RoleAdmin)]
    public class AdminController : CRUDController
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View(Repository.EditableUsers.ToList().ConvertAll(u =>
            {

                var m = CreateEmptyModel();
                m.User = u;
                return m;
            }));
        }

        Models.CreateUserModel CreateEmptyModel()
        {
            return new Models.CreateUserModel() {Roles = Repository.Roles.Select(r => new Models.CheckedRole() { Role = r }).ToList() };
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View(CreateEmptyModel());
        }

        // POST: Admin/Create
        [HttpPost]
        public ActionResult Create(Models.CreateUserModel Model, FormCollection Form)
        {
            var act = new Action(() =>
            {
                var dbRoles = Repository.Roles;

                int c = Math.Min(Model.Roles.Count, dbRoles.Count);
                for (int i = 0; i < c; ++i)
                    Model.Roles[i].Role = dbRoles[i];

                var newUser = new Entities.GlUser();
                newUser.Name = Model.User.Name;
                newUser.Password = Model.User.Password;
                newUser.Email = Model.User.Email;

                var roles = CreateEmptyModel().GetRoles(User.IsInRole(Algowe.Web.Global.AConst.RoleSuperAdmin));
                for (int i = 0; i < roles.Count; ++i) // suggestion: order of roles same as in Create.cshtml
                {
                    var role = roles[i];
                    var r = Model.Roles[i];
                    if (r.Checked)
                        newUser.UserRoles.Add(new Entities.GlUserRole() { CurrentUser = newUser, CurrentRole = role.Role });
                }
                Repository.CreateUser(newUser);
            });

            return Create(act, CreateEmptyModel);
        }

        // GET: Admin/Edit/5
        public ActionResult Details(int id)
        {
            var m = CreateEmptyModel();
            m.User = Repository.GetUser(id);
            return View(m);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Edit/5
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

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            var userToDel = Repository.GetUser(id);
            var notDelIfPredicateIsTrue = new Func<bool>(() =>
            {
                var users = Repository.Users.ToList();
                var frozenUsers = users.FindAll(u => u.Name == Auth.CurrentUser.Identity.Name || u.UserRoles.ToList().Exists(ur => ur.CurrentRole.Name == Global.AConst.RoleSuperAdmin));
                return frozenUsers.Exists(u => u.Id == userToDel.Id);
            });
            return Delete2(id, notDelIfPredicateIsTrue, Repository.RemoveUser, "Unable to delete user " + userToDel.Name);
        }
    }
}
