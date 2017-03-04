using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Reflection;

using Autofac;
using Autofac.Integration.Mvc;
using NHibernate.Linq;

using Algowe.Web.Global;
using Algowe.Web.Entities;

namespace Algowe.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Account", action = "LogOn", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
			
            // Setup IoC container
			ContainerBuilder builder = Factory.IoContainer;
            builder.RegisterType<Models.SqlRepository>().As<Models.IRepository>().InstancePerRequest();
            builder.RegisterType<CustomAuthentication>().As<IAuthentication>().PropertiesAutowired().InstancePerRequest();
            

            //builder.Register(c => new CustomAuthentication ()).As<IAuthentication> ().InstancePerRequest();

            //Assembly executingAssembly = Assembly.GetExecutingAssembly();

            // Register dependencies in controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();

			// Register dependencies in filter attributes
			builder.RegisterFilterProvider();

			// Register dependencies in custom views
			builder.RegisterSource(new ViewRegistrationSource());

            //builder.Register(c => new Algowe.Web.Controllers.AccountController().

            IContainer container = builder.Build();

			var afdr = new AutofacDependencyResolver(container);

			DependencyResolver.SetResolver(afdr);


			AreaRegistration.RegisterAllAreas();
			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);

            //builder.RegisterModule(new ServicesModule());

            //builder.Register(c => new Entities()).As<IEntities>()
            //	.OnActivated(e =>
            //		{
            //			var config = DependencyResolver.Current.GetService<IGlobalSettings>();
            //			e.Instance.RawConnectionString = config.Data.SqlServerConnectionString;
            //		}).InstancePerHttpRequest();
            /*
			builder.RegisterType<LocalizationContext>().PropertiesAutowired().InstancePerHttpRequest();

			builder.Register(c => new DefaultGlobalSettings()).As<IGlobalSettings>().SingleInstance();

			builder.RegisterGeneric(typeof(MongoRepository<>))
				.As(typeof(IMongoRepository<>)).InstancePerHttpRequest();

			builder.Register(c => EnvironmentContext()).As<IEnvironmentContext>().InstancePerHttpRequest();

			builder.RegisterModule(new AutofacWebTypesModule());

			builder.RegisterSource(new ViewRegistrationSource());

			builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
			builder.RegisterModelBinderProvider();

			builder.RegisterFilterProvider();

			IContainer container = builder.Build();
			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));*/
            CheckVersion();
        }

        void CheckVersion()
        {
            NHibernateHelper.MakeTransaction(s =>
            {
                var linq = from config in s.Query<GlConfig>() select config;
                if (linq.Count() < 1 || linq.ToList()[0].Version < 1)
                {

                    #region Global
                    var configs = linq.ToList();
                    var conf = configs.Count < 1 ? new GlConfig() : configs[0];
                    conf.Version = 1;
                    s.SaveOrUpdate(conf);

                    foreach (var role in AConst.Roles)
                    {
                        var r = new GlRole() { Name = role };
                        s.SaveOrUpdate(r);
                    }

                    CreateUser(s, "root", "asd", AConst.RoleSuperAdmin);
                    CreateUser(s, "admin", "1", AConst.RoleAdmin);
                    #endregion

                    #region HR
                    foreach (var d in AConst.Departments)
                        s.Save(new HrDepartment() { Name = d.Item1 });
                    var depsFromBase = s.Query<HrDepartment>();
                    foreach (var d in AConst.Departments.Where(_ => !string.IsNullOrWhiteSpace(_.Item2)))
                    {
                        var depFromBase = depsFromBase.FirstOrDefault(_ => _.Name == d.Item1);
                        if (depFromBase != null)
                        {
                            depFromBase.HeadDepartment = depsFromBase.FirstOrDefault(_ => _.Name == d.Item2);
                            s.SaveOrUpdate(depFromBase);
                        }
                    }

                    #endregion
                }
            });
        }

        void CreateUser(NHibernate.ISession s, string Name, string Pass, string RoleName)
        {
            if (!AConst.Roles.Contains(RoleName))
                throw new Exception("Unkown role" + RoleName);
            var newUserRole = (from role in s.Query<GlRole>() where role.Name == RoleName select role).ToList().FirstOrDefault();
            if (newUserRole==null)
                throw new Exception("Unable to find role " + RoleName + " in databases");
            s.SaveOrUpdate(new GlUser() { Name = Name, Password = Pass });
            var createdUser = (from item in s.Query<GlUser>() where item.Name == Name select item).ToList().FirstOrDefault();
            if (createdUser == null)
                throw new Exception("Unable to create user " + Name + " with role " + RoleName);
            createdUser.UserRoles = new List<GlUserRole>() { new GlUserRole() { CurrentUser = createdUser, CurrentRole = newUserRole } };
            s.SaveOrUpdate(createdUser);
        }




            
    }

	public static class Factory
	{
		static public ContainerBuilder IoContainer {
			get{ return Singleton<ContainerBuilder>.Instance; }
		}

	}


	public sealed class Singleton<T>  where T : class, new ()
	{
		private static volatile T instance;
		private static object syncRoot = new Object();

		private Singleton() {}

		public static T Instance
		{
			get 
			{
				if (instance == null) 
				{
					lock (syncRoot) 
					{
						if (instance == null) 
							instance = new T();
					}
				}

				return instance;
			}
		}
	}
}