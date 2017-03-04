using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Algowe.Web.Entities
{
    public class HrDepartment
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual HrDepartment HeadDepartment { get; set; }
    }
}