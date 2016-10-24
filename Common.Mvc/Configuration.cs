using System.Web.Mvc;
using Common.Mvc.Filters;
using log4net;

namespace Common.Mvc
{
    public class Configuration
    {
        public static void Configure()
        {
            RegisterErrorFilter(GlobalFilters.Filters);
        }

        private static void RegisterErrorFilter(GlobalFilterCollection filters)
        {
            filters.Add(new HandleExceptionsAttribute(LogManager.GetLogger(typeof(HandleExceptionsAttribute).Name)));
            filters.Add(new CrosAttribute());
        }
    }
}
