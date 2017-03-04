using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Algowe.Web.Global
{
    static public class AConst
    {
        static public string[] Roles { get { return new[] { RoleSuperAdmin, RoleAdmin, RoleUser }; } } 
        public const string RoleSuperAdmin = "SuperAdmin";
        public const string RoleAdmin = "Admin";
        public const string RoleUser = "User";

        static public Tuple<string, string>[] Departments { get { return new[] {
            new Tuple<string, string>("Department1", null),
            new Tuple<string, string>("Department2", null),
            new Tuple<string, string>("Department3", "Department2"),
            new Tuple<string, string>("Department4", "Department2") }; } }
    }
}