using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTBGD.Models;
using PagedList.Mvc;
using PagedList;

namespace WebTBGD.Areas.Admin.Controllers
{
    public class TimKiemController : Controller
    {
        // GET: Admin/TimKiem
        DBTHIETBIGIADUNGDataContext db = new DBTHIETBIGIADUNGDataContext();
        // GET: TimKiem
        [HttpPost]
        public ActionResult KetQuaTimKiemKH(FormCollection f, int? page)
        {
            string sTuKhoa = f["txttimkiem"].ToString();
            ViewBag.TuKhoa = sTuKhoa;
            List<CTDATHANG> lstKQTK = db.CTDATHANGs.Where(n => n.DONDATHANG.KHACHHANG.TENKH.Contains(sTuKhoa)).ToList();
            int pageNumber = (page ?? 1);
            int pageSize = 12;
            if (lstKQTK.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy!!!";

            }
            ViewBag.ThongBao = "Đã tìm thấy " + lstKQTK.Count + " kết quả";
            return View(lstKQTK.OrderBy(n => n.DONDATHANG.KHACHHANG.TENKH).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult KetQuaTimKiemKH(int? page, string sTuKhoa)
        {
            ViewBag.TuKhoa = sTuKhoa;
            List<CTDATHANG> lstKQTK = db.CTDATHANGs.Where(n => n.DONDATHANG.KHACHHANG.TENKH.Contains(sTuKhoa)).ToList();
            int pageNumber = (page ?? 1);
            int pageSize = 12;
            if (lstKQTK.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy !!!";
                
            }
            ViewBag.ThongBao = "Đã tìm thấy " + lstKQTK.Count + " kết quả";
            return View(lstKQTK.OrderBy(n => n.DONDATHANG.KHACHHANG.TENKH).ToPagedList(pageNumber, pageSize));
        }
    }
}