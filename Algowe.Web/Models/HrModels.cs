using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Algowe.Web.Entities;

namespace Algowe.Web.Models
{
    public class HrPersonModel
    {
        public HrPerson Person { get; set; }
        public List<HrDepartment> Departments { get; set; }
        public List<System.Web.Mvc.SelectListItem> ModelDepartments { get { return Departments.ConvertAll(d => new System.Web.Mvc.SelectListItem() { Text = d.Name, Value = d.Id.ToString() }); } }
        public System.Web.Mvc.SelectListItem SelectedDepartment { get; set; }
    }
}