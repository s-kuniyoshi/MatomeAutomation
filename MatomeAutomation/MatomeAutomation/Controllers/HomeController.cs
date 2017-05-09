using MatomeAutomation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MatomeAutomation.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        // POST: /Home/Result
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Result(MyProcessThread model)
        {
            if (ModelState.IsValid)
            {
                var result = new MyProcessThread { Html = model.Html, ExtractRes = model.ExtractRes };
                ViewBag.html = result.GetResList(result.Html);
                return View();
            }

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

    }
}