using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XDDEasy.Domain.ResourceAggregates;

namespace XDDEasy.Main.Areas.Admin.Controllers
{
    public class ResourceController : Controller
    {
        // GET: Admin/Resource
        public ActionResult Index()
        {
            return View();
        }
    }
}