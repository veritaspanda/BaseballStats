using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaseballStats.Common;
using BaseballStats.DataAccess;

namespace BaseballStats.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult PlayerSearch()
        {
            ViewBag.Message = "Test";
            return View(DataAccess.Common.TestGetPlayerStats());
        }
    }
}