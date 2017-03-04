using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PGK.Extensions;
using NHibernate.Linq;

using Algowe.Web.Entities;
using Algowe.Web.Global;

namespace Algowe.Web.Models
{
    public partial class SqlRepository : IRepository
    {


        #region Global
        #region Role
        static IList<GlRole> roles;
        public IList<GlRole> Roles
        {
            get
            {
                if (roles == null)
                    roles = NHibernateHelper.GetList<GlRole>();
                return roles;
            }
        }
        #endregion

        #region User



        public IList<GlUser> Users
        {
            get
            {
                return NHibernateHelper.GetList<GlUser>(null, (res, s) => res.ForEach(r => InitUserRoles(r, s)));
            }
        }

        public IEnumerable<GlUser> EditableUsers
        {
            get
            {
                return Users.Where(_ => !_.InRoles(AConst.RoleSuperAdmin));
            }
        }

        void InitUserRoles(GlUser r, NHibernate.ISession s)
        {
            r.UserRoles = (from item in s.Query<GlUserRole>() where item.CurrentUser.Id == r.Id select item).ToList();
            NHibernate.NHibernateUtil.Initialize(r.UserRoles);
            foreach (var ur in r.UserRoles)
                ur.CurrentRole = Roles.ToList().Find(r1 => r1.Id == ur.CurrentRole.Id);
        }

        public bool CreateUser(GlUser instance)
        {
            return NHibernateHelper.Create<GlUser>(instance, NotAddIfPredicateIsTrue: _ => string.IsNullOrWhiteSpace(_.Name), ExistsInBase: _ => _.Name == instance.Name);
        }

        public bool UpdateUser(GlUser instance)
        {
            return NHibernateHelper.UpdateItem<GlUser>(_ => _.Id == instance.Id, instance);
        }

        public bool RemoveUser(int idUser)
        {
            return NHibernateHelper.RemoveItem<GlUser>(_ => _.Id == idUser);
        }


        public GlUser GetUser(string Name)
        {
            return NHibernateHelper.GetItem<GlUser>(p => p.Name == Name, InitUserRoles);
        }

        public GlUser GetUser(int Id)
        {
            return NHibernateHelper.GetItem<GlUser>(p => p.Id == Id, InitUserRoles);
        }

        public GlUser Login(string Name, string password)
        {
            return NHibernateHelper.GetItem<GlUser>(p => p.Name == Name && p.Password == password, InitUserRoles);
        }
        #endregion

        #region UserRole
        public IList<GlUserRole> UserRoles
        {
            get
            {
                return NHibernateHelper.GetList<GlUserRole>();
            }
        }
        #endregion

        #endregion

        #region HR

        #region Persons

        public HrPerson GetPerson(long UniqID)
        {
            return NHibernateHelper.GetItem<HrPerson>(h => h.UniqID == UniqID, (res, s) => NHibernate.NHibernateUtil.Initialize(res.Department));
        }

        public bool CreatePerson(HrPerson instance)
        {
            return NHibernateHelper.Create<HrPerson>(instance, h => h.UniqID == 0, h => h.UniqID == instance.UniqID);
        }

        public bool UpdatePerson(HrPerson instance)
        {
            return NHibernateHelper.UpdateItem<HrPerson>(_ => _.Id == instance.Id, instance);
        }

        public bool RemovePerson(int idPerson)
        {
            return NHibernateHelper.RemoveItem<HrPerson>(_ => _.Id == idPerson);
        }

        //static IList<HrHuman> persons;D:\projects\AS KIO\algowe\algowe\Algowe.Web\Controllers\HomeController.cs
        public IList<HrPerson> Persons
        {
            get
            {
                //if (humans == null)
                var persons = NHibernateHelper.GetList<HrPerson>(null, (res, s) => res.ForEach(_ => NHibernate.NHibernateUtil.Initialize(_.Department)));
                return persons;
            }
        }


        #endregion

        #region Departments
        static IList<HrDepartment> departments;
        public IList<HrDepartment> Departments
        {
            get
            {
                if (departments == null)
                    departments = NHibernateHelper.GetList<HrDepartment>(null, (res, s) => res.ForEach(_ => NHibernate.NHibernateUtil.Initialize(_.HeadDepartment)));
                return departments;
            }
        }
        #endregion


        #endregion
    }
}
