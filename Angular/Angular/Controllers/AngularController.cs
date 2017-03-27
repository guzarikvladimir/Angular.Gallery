using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Angular.Controllers
{
    public class AngularController : Controller
    {
        public ActionResult Index(string url)
        {
            return View("Index");
        }
    }
}
