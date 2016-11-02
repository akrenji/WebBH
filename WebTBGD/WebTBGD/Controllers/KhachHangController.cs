using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTBGD.Models;
namespace WebTBGD.Controllers
{
    public class KhachHangController : Controller
    {
        DBTHIETBIGIADUNGDataContext db = new DBTHIETBIGIADUNGDataContext();
        // GET: KhachHang
        public ActionResult Index()
        {   
            return View();
        }
        [HttpGet]
        public ActionResult DangkyKH()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult DangkyKH(FormCollection collection, KHACHHANG kh)
        {
            var email = collection["Username"];
            var matkhau = collection["Password"];
            var dienthoai = collection["Phone"];
            var ten = collection["NAME"];
            var gioitinh = collection["GIOITINH"];
            var diachi = collection["DIACHI"];
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không được để trống";
            }
         
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi4"] = "Phải nhập lại mật khẩu";
            }


            if (String.IsNullOrEmpty(ten))
            {
                ViewData["Loi5"] = "Email không được bỏ trống";
            }

            if (String.IsNullOrEmpty(diachi))
            {
                ViewData["Loi6"] = "Phải nhập đia chỉ";
            }
            else
            {
                kh.DIACHI = diachi;
                kh.EMAIL = email;
                kh.PASS = matkhau;
                kh.SDT = dienthoai;
                kh.GIOITINH = gioitinh;
                kh.TENKH = ten;
                db.KHACHHANGs.InsertOnSubmit(kh);
                db.SubmitChanges();
            }
            return this.Dangnhap();

        }
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
        {
            var email = collection["ID"];
            var matkhau = collection["PASS"];
          
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi1"] = "Phải Nhập Email";
            

            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải mật khẩu";

            }
            else
            {
                KHACHHANG KH = db.KHACHHANGs.SingleOrDefault(N => N.EMAIL == email && N.PASS == matkhau);
        
                if (KH != null)
                {
                    ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                    Session["User"] = KH;
                    ViewBag.thanhvien = KH.TENKH;
                    return RedirectToAction("Index", "Home");
      
                }
                else
                {
                    ViewBag.Thongbao = "Tên đăng nhập sai";
                    return RedirectToAction("Dangnhap");
                }
            }
            return View();
        }

        public ActionResult Thoat()
        {
            Session["User"] = null;
            return RedirectToAction("Index", "Home");
            
        }
        public ActionResult ThongTinTaiKhoan()
        {
            var kh = (KHACHHANG)Session["User"];
            return View(kh);
        }
    }
}