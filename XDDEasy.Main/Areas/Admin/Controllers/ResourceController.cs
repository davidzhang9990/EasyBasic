using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XDDEasy.Domain.ResourceAggregates;
using XDDEasy.Main.Controllers;

namespace XDDEasy.Main.Areas.Admin.Controllers
{
    public class ResourceController : EasyMvcBaseController
    {
        // GET: Admin/Resource
        public ActionResult Index()
        {
            ViewBag.addDisplay = AddDisplay;
            ViewBag.editDisplay = EditDisplay;
            ViewBag.delDisplay = DelDisplay;
            return View();
        }

        public ActionResult Edit(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty)
            {
                return RedirectToAction("Index", "Resource", new { area = "Admin" });
            }
            return View();
        }

        public ActionResult Add(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty)
            {
                return View("Edit");
            }
            return RedirectToAction("Index", "Resource", new { area = "Admin" });
        }
    }
}