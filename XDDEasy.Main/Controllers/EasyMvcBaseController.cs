using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Common.Helper;
using Common.Mvc;
using log4net;
using WebGrease.Css.Extensions;

namespace XDDEasy.Main.Controllers
{
    [HandleError]
    public abstract class EasyMvcBaseController : Controller
    {
        public int PageSize
        {
            get
            {
                int defaultPageSize = 10;
                string pageSize = ConfigurationManager.AppSettings["PageDefaultCount"];
                if (!string.IsNullOrEmpty(pageSize))
                {
                    int.TryParse(pageSize, out defaultPageSize);
                }
                return defaultPageSize;
            }
        }

        //refrence: http://afana.me/post/aspnet-mvc-internationalization.aspx
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            var lang = LangHelper.GetLanguage();

            SetCulture(Request, lang);
            return base.BeginExecuteCore(callback, state);
        }


        private void SetCulture(HttpRequestBase request, string lang)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Response.Buffer = true;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
            Response.Expires = 0;
            Response.CacheControl = "no-cache";
            Response.Cache.SetNoStore();

            base.OnActionExecuted(filterContext);
        }
    }
}