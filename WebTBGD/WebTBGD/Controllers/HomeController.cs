using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTBGD.Models;

namespace WebTBGD.Controllers
{
    public class HomeController : Controller
    {
        DBTHIETBIGIADUNGDataContext db = new DBTHIETBIGIADUNGDataContext();
        public ActionResult Index()
        {
            var sp = LAYSP(4);
            return View(sp);
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
        public ActionResult menu()
        {
            var a = from tt in db.LOAISPs select tt;
            return PartialView(a);
        }
        public ActionResult bottom()
        {
            return PartialView();
        }
        private List<SANPHAM> LAYSP(int count)
        {
            return db.SANPHAMs.OrderByDescending(a => a.MASP).Take(count).ToList();
        }


    }
}