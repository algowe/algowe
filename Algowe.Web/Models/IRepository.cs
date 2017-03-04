using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Reflection;
using System.Linq.Expressions;
using System.ComponentModel;

using Algowe.Web.Entities;

namespace Algowe.Web.Models
{
    public interface IRepository
    {
        #region Role
        IList<GlRole> Roles { get; }
        #endregion

        #region User

        IList<GlUser> Users { get; }

        IEnumerable<GlUser> EditableUsers { get; }

        bool CreateUser(GlUser instance);

        bool UpdateUser(GlUser instance);

        bool RemoveUser(int idUser);
        

        GlUser GetUser(int Id);
        GlUser GetUser(string Name);

        GlUser Login(string email, string password);

        #endregion 
        
        #region UserRole

        IList<GlUserRole> UserRoles { get; }

        /*bool CreateUserRole(GlUserRole instance);

        bool UpdateUserRole(GlUserRole instance);

        bool RemoveUserRole(int idUserRole);*/

        #endregion

        #region HR
        bool CreatePerson(HrPerson instance);
        HrPerson GetPerson(long UniqID);
        bool UpdatePerson(HrPerson instance);
        IList<HrPerson> Persons { get; }
        IList<HrDepartment> Departments { get; }
        bool RemovePerson(int idPerson);
        #endregion
    }
}
