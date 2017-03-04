using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

using Algowe.Web.Models;
using Algowe.Web.Entities;

namespace Algowe.Web
{
    /// <summary>
    /// Реализация интерфейса для идентификации пользователя
    /// </summary>
    public class UserIndentity : IIdentity, IUserProvider
    {
        /// <summary>
        /// Текщий пользователь
        /// </summary>
        public GlUser User { get; set; }

        /// <summary>
        /// Тип класса для пользователя
        /// </summary>
        public string AuthenticationType
        {
            get
            {
                return typeof(GlUser).ToString();
            }
        }

        /// <summary>
        /// Авторизован или нет
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                return User != null;
            }
        }

        /// <summary>
        /// Имя пользователя (уникальное) [у нас это счас Email]
        /// </summary>
        public string Name
        {
            get
            {
                if (User != null)
                {
                    return User.Name;
                }
                //иначе аноним
                return "anonym";
            }
        }

        /// <summary>
        /// Инициализация по имени
        /// </summary>
        /// <param name="Name">имя пользователя [Name]</param>
        public void Init(string Name, IRepository repository)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                User = repository.GetUser(Name);
            }
        }
    }
}