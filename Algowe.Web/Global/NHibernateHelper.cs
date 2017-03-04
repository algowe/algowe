using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Linq;
using NHibernate.Tool.hbm2ddl;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

using Algowe.Web;

namespace Algowe.Web.Global
{
    public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                    InitializeSessionFactory();
                return _sessionFactory;
            }
        }

        private static void InitializeSessionFactory()
        {
			try{
			string server = "localhost";
            string port = "5432";
            string database = "algowe_db";
            string user = "postgres";
            string password = "pg78";

                string connectionString = "Server=" + server + ";" +
                    "Port=" + port + ";" +
                    "User Id=" + user + ";" +
                    "Password=" + password + ";" +
                    "Database=" + database + ";";
                bool withAutoMap = true;
                if (!withAutoMap)
                    _sessionFactory = Fluently.Configure()
                    .Database(FluentNHibernate.Cfg.Db.PostgreSQLConfiguration.Standard
                              .ConnectionString(connectionString)
                              .ShowSql()
                        )
                        .Mappings(m =>
                          m.FluentMappings
                            .AddFromAssemblyOf<Algowe.Web.Entities.GlUser>())
                    .ExposeConfiguration(TreatConfiguration)
                .BuildSessionFactory();
                else
                {
                    _sessionFactory = Fluently.Configure()
                        .Database(FluentNHibernate.Cfg.Db.PostgreSQLConfiguration.Standard
                            .ConnectionString(connectionString)
                            .ShowSql()
                        )
                        .Mappings(m =>
                            m.AutoMappings.Add(
                                FluentNHibernate.Automapping.AutoMap.AssemblyOf<Entities.GlUser>()
                                .Where(t => t.Namespace == "Algowe.Web.Entities")
                                .Override<Entities.GlUser>(map => map.HasMany(x => x.UserRoles).Cascade.All())
                            ))
                        .ExposeConfiguration(TreatConfiguration)
                        .BuildSessionFactory();


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

		private static void TreatConfiguration(Configuration configuration)
		{
			// dump sql file for debug
			Action<string> updateExport = x => {
				using (var file = new System.IO.FileStream (@"update.sql", System.IO.FileMode.Append))
				using (var sw = new System.IO.StreamWriter (file)) {
					sw.Write (x);
					sw.Close ();
				}
			};

			var update = new SchemaUpdate (configuration);
			update.Execute (updateExport, true);
		}
		
        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        static public void MakeTransaction(Action<ISession> Act)
        {

            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {

                    Act(session);
                    transaction.Commit();
                }
                session.Close();
            }
        }

        static public bool Create<T>(T instance, Predicate<T> NotAddIfPredicateIsTrue, Expression<Func<T, bool>> ExistsInBase)
        {
            if (NotAddIfPredicateIsTrue(instance))
                return false;
            else
            {
                bool res = false;
                NHibernateHelper.MakeTransaction(s =>
                {
                    var item = s.Query<T>().Where(ExistsInBase).FirstOrDefault();
                    if (item != null)
                        res = false;
                    else
                    {
                        s.Save(instance);
                        res = true;
                    }
                });
                return res;
            }
        }

        static public bool RemoveItem<T>(Expression<Func<T, bool>> Match) where T : class
        {
            return MakeAction(Match, (item, s) => s.Delete(item));
        }

        static public bool UpdateItem<T>(Expression<Func<T, bool>> Match, T NewInstance) where T : class
        {
            return MakeAction(Match, (item, s) => s.SaveOrUpdate(item), NewInstance);
        }

        static bool MakeAction<T>(Expression<Func<T, bool>> FindItemMatch, Action<T, ISession> Act, T ActArg1 = null) where T : class
        {
            bool res = false;
            NHibernateHelper.MakeTransaction(s =>
            {
                var item = s.Query<T>().Where(FindItemMatch).FirstOrDefault();
                res = item != null;
                if (ActArg1 != null)
                    Act(ActArg1, s);
                else
                    Act(item, s);
            });
            return res;
        }

        static public T GetItem<T>(Expression<Func<T, bool>> Match, Action<T, ISession> Act = null)
        {

            var res = default(T);
            NHibernateHelper.MakeTransaction(s =>
            {
                res = s.Query<T>().Where(Match).FirstOrDefault();
                NHibernate.NHibernateUtil.Initialize(res);
                if (Act != null && res != null)
                    Act(res, s);
            });

            return res;
        }


        static public IList<T> GetList<T>(Expression<Func<T, bool>> Match = null, Action<IEnumerable<T>, ISession> Act = null)
        {
            var res = new List<T>();
            NHibernateHelper.MakeTransaction(s =>
            {
                res = (Match != null ? s.Query<T>().Where(Match) : s.Query<T>()).ToList();
                NHibernate.NHibernateUtil.Initialize(res);
                if (Act != null)
                    Act(res, s);
            });

            return res;
        }

    }

}
