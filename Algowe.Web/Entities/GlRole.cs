using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Algowe.Web.Entities
{
    public class GlRole
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; } 
    }

}
