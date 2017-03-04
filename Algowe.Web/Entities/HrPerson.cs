using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Algowe.Web.Entities
{
    public class HrPerson
    {
        public virtual int Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string SecondName { get;set;}
        public virtual string LastName { get;set;}
        public virtual long UniqID { get; set; }
        public virtual HrDepartment Department { get; set; }
    }
}