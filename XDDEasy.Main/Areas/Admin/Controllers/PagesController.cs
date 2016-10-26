using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XDDEasy.Main.Controllers;

namespace XDDEasy.Main.Areas.Admin.Controllers
{
    public class PagesController : EasyMvcBaseController
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            //ViewBag.addDisplay = AddDisplay;
            //ViewBag.editDisplay = EditDisplay;
            //ViewBag.delDisplay = DelDisplay;
            return View();
        }

        public ActionResult Edit(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty)
            {
                return RedirectToAction("Index", "Pages", new { area = "Admin" });
            }
            return View();
        }

        public ActionResult Add(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty)
            {
                return View("Edit");
            }
            return RedirectToAction("Index", "Pages", new { area = "Admin" });
        }
    }
}