using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
