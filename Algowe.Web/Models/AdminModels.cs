using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Algowe.Web.Models
{
    public class CreateUserModel
    {
        public Entities.GlUser User { get; set; } = new Entities.GlUser();
        public List<CheckedRole> Roles { get; set; } = new List<CheckedRole>();
        public List<CheckedRole> CommonRoles { get { return Roles.FindAll(r => r.Role != null && r.Role.Name != Global.AConst.RoleSuperAdmin); } }
        public List<CheckedRole> GetRoles(bool IsSuperAdmin) { return IsSuperAdmin ? Roles : CommonRoles; }
    }

    public class CheckedRole
    {
        public Entities.GlRole Role { get; set; }
        public bool Checked { get; set; }
    }
}