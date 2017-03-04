using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Algowe.Web.Models;
using Algowe.Web.Entities;

namespace Algowe.Web
{
    public interface IUserProvider
    {
        GlUser User { get; set; }
    }
}
