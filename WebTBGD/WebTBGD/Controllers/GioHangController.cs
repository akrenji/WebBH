using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTBGD.Models;
namespace WebTBGD.Controllers
{
    public class GioHangController : Controller
    {
        DBTHIETBIGIADUNGDataContext db = new DBTHIETBIGIADUNGDataContext();
        
        public List<GioHang> Laygiohang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if(lstGioHang==null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        public ActionResult ThemGioHang(int iMASP,string strURL)
        {
            List<GioHang> lstGioHang = Laygiohang();
            GioHang sp = lstGioHang.Find(n => n.iMASP == iMASP);
            if(sp == null)
            {
                sp = new GioHang(iMASP);
                lstGioHang.Add(sp);
                return Redirect(strURL);
            }
            else
            {
                sp.iSOLUONG++;
                return Redirect(strURL);
            }
        }
        public int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if(lstGioHang!=null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.iSOLUONG);

            }
            return iTongSoLuong;
        }
        private double TongTien()
        {
            double iTongTien = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if(lstGioHang != null)
            {
                iTongTien = lstGioHang.Sum(n => n.THANHTIEN);

            }
            return iTongTien;
        }
        public ActionResult GioHang()
        {   
            List<GioHang> lstGioHang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }
         
        // GET: GioHang
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult XoaGioHang(int iMASP)
        {
            List<GioHang> lstGioHang = Laygiohang();
            GioHang sp = lstGioHang.SingleOrDefault(n => n.iMASP == iMASP);
            if(sp != null)
            {
                lstGioHang.RemoveAll(n => n.iMASP == iMASP);
                return Redirect("GioHang");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapNhapGioHang(int iMASP,FormCollection F)
        {
            List<GioHang> lstGioHang = Laygiohang();
            GioHang sp = lstGioHang.SingleOrDefault(n => n.iMASP == iMASP);
            if(sp != null)
            {
                sp.iSOLUONG=int.Parse(F["txtSoLuong"].ToString());
                return Redirect("GioHang");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaTatcaGioHang()
        {
            List<GioHang> lstGioHang = Laygiohang();
            lstGioHang.Clear();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            //Kiem tra dang nhap
            if (Session["User"] == null || Session["User"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "KhachHang");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

           
            List<GioHang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();

            return View(lstGiohang);
        }
        
        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
           
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["User"];
            List<GioHang> gh = Laygiohang();
            ddh.STT = kh.STT;
            ddh.NGAYDAT = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NGAYGIAO"]);
            ddh.NGAYGIAO = DateTime.Parse(ngaygiao);
            ddh.TINHTRANGGIAOHANG = false;
            ddh.DATHANHTOAN = false;
            db.DONDATHANGs.InsertOnSubmit(ddh);
            db.SubmitChanges();
                      
            foreach (var item in gh)
            {
                CTDATHANG ctdh = new CTDATHANG();
                ctdh.MADH = ddh.MADH;
                ctdh.MASP = item.iMASP;
                ctdh.SOLUONG = item.iSOLUONG;
                ctdh.DONGIA = (decimal)item.iGIA;
                db.CTDATHANGs.InsertOnSubmit(ctdh);
            }
            db.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");
        }
        public ActionResult Xacnhandonhang()
        {
            return View();
        }
        public ActionResult Top()
        {
            List<GioHang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView(lstGiohang);
        }

    }
}