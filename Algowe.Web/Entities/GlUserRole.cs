using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

using Algowe.Web.Entities;

namespace Algowe.Web.Entities
{
    public class GlUserRole
    {
        public virtual int Id { get; set; }
        public virtual GlRole CurrentRole { get; set; }
        public virtual GlUser CurrentUser { get; set; }

        public GlUserRole()
        {
            CurrentRole = default(GlRole);
            CurrentUser = default(GlUser);
        }

        public GlUserRole(GlRole R, GlUser U)
        {
            CurrentRole = R;
            CurrentUser = U;
        }
    }

}
