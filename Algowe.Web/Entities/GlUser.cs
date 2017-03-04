using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Algowe.Web.Entities
{
    public class GlUser
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Password { get; set; }
        public virtual string Email { get; set; }
        public virtual IList<GlUserRole> UserRoles { get; set; } = new List<GlUserRole>();

        public virtual bool InRoles(string roles)
        {
            if (string.IsNullOrWhiteSpace(roles))
            {
                return false;
            }

            var rolesArray = roles.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var role in rolesArray)
            {
                var hasRole = UserRoles.Any(p => p.CurrentRole.Name == role);
                if (hasRole)
                {
                    return true;
                }
            }
            return false;
        }

    }
}