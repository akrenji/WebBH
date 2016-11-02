using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTBGD.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
namespace WebTBGD.Controllers
{
    public class SanPhamController : Controller
    {
        DBTHIETBIGIADUNGDataContext db = new DBTHIETBIGIADUNGDataContext();
        // GET: SanPham
        public ActionResult Index(int id)
        {   
            var sp = from t in db.SANPHAMs
                     where t.MASP == id
                     select t;
            return View(sp.Single());
       
        }
 
        public ActionResult ThongtinSP(int id)
       {
          var sp = from t in db.SANPHAMs
                   where t.MASP == id
                   select t;
             return View(sp.Single());
       }
        public ActionResult SPmenu(int id, int? page)
        {
            int pageSize = 16;
            int pageNum = (page ?? 1);
            var a = from tt in db.SANPHAMs where tt.MALOAI == id select tt;
            LOAISP loai = db.LOAISPs.SingleOrDefault(N => N.MALOAI == id);
            ViewBag.loai = loai.TENLOAI;
            return View(a.ToPagedList(pageNum, pageSize));
        }
        public ActionResult SPDanhmuc(int id, int? page)
        {
            int pageSize = 16;
            int pageNum = (page ?? 1);
            var a = from tt in db.SANPHAMs where tt.LOAISP.DANHMUC.MADM == id select tt;
            DANHMUC dm = db.DANHMUCs.SingleOrDefault(N => N.MADM == id);
            ViewBag.DM = dm.TENDANHMUC;
     
            return View(a.ToPagedList(pageNum, pageSize));
        }
        public ActionResult NgauNhien()
        {
            var ngaunhien = db.SANPHAMs.GetRandomItems(4);
            return View(ngaunhien);
        }
        public ActionResult LienQuan(int id)
        {
            var lienquan = db.SANPHAMs.First(n => n.MASP == id);
            var lq = db.SANPHAMs.Where(n => n.MALOAI == lienquan.MALOAI).ToList();
            return PartialView(lq);
        }


    }
}