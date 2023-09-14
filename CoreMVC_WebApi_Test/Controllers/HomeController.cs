using CoreMVC_WebApi_Test.Data;
using CoreMVC_WebApi_Test.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CoreMVC_WebApi_Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PubsContext _context;

        public HomeController(ILogger<HomeController> logger, PubsContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Titles()
        {
            return View();
        }

        public ActionResult CreateTitle()
        {
            var pubIds = (from p in _context.Publishers select p.PubId).ToList();
            var pubIdItems = pubIds.Select(pubId => new SelectListItem
            {
                Text = pubId,
                Value = pubId
            });

            ViewBag.PubId = new SelectList(pubIdItems, "Value", "Text");
            return View();
        }

        public ActionResult CreatePublisher()
        {
            return View();
        }

        public ActionResult EditTitle(string id)
        {
            var title = (from t in _context.Titles
            where t.TitleId == id
            select t).FirstOrDefault();

            var pubIds = (from p in _context.Publishers select p.PubId).ToList();
            var pubIdItems = pubIds.Select(pubId => new SelectListItem
            {
                Text = pubId,
                Value = pubId
            });

            ViewBag.PubId = new SelectList(pubIdItems, "Value", "Text", title.PubId);

            if (title == null)
                return RedirectToAction("Index", "Au");

            return View("EditTitle", title);
        }

        public ActionResult Publishers()
        {
            return View();
        }

        public ActionResult EditPublisher(string id)
        {
            var pub = (from p in _context.Publishers
                       where p.PubId == id
                       select p).FirstOrDefault();

            if (pub == null)
                return RedirectToAction("Index", "Au");

            return View("EditPublisher", pub);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}