using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaseballStats.Common;
using BaseballStats.DataAccess;
using BaseballStats.Models;

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
            ViewBag.Message = "Player Search";
            return View(DataAccess.Common.TestGetPlayerStats());
        }

        public ActionResult PlayerSearchInput()
        {
            ViewBag.Message = "Player Search Input";
            return View();
        }

        [HttpPost]
        public ActionResult PlayerSearchInfo(PlayerSearchInputModel model)
        {
            ViewBag.Message = "Player Search Info";

            string _firstName = string.Empty;
            string _lastName = string.Empty;

            if (ModelState.IsValid)
            {
                _firstName = model.PlayerFirstName;
                _lastName = model.PlayerLastName;

                if(_firstName != null && _firstName != "" && _lastName != null && _lastName != "")
                {
                    _firstName = _firstName.ToLower();
                    _lastName = _lastName.ToLower();
                }
                else if(_lastName != null && _lastName != "")
                {
                    string _tempStr = "%25";
                    _lastName = _lastName.ToLower() + _tempStr;
                    _firstName = "NOTVALID";
                }
                else
                {
                    _lastName = "granderson%25";
                    _firstName = "NOTVALID";
                }
            }

            return View("PlayerSearch",DataAccess.Common.PromptedGetPlayerStats(_firstName,_lastName));
        }
    }
}