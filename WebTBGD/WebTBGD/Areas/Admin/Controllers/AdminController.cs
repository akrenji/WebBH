using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTBGD.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
namespace WebTBGD.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        DBTHIETBIGIADUNGDataContext db = new DBTHIETBIGIADUNGDataContext();
        // GET: Admin/Admin
      
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        } 
        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            var ten = collection["id"];
            var matkhau = collection["pass"];
            if(String.IsNullOrEmpty(ten))
            {
                ViewData["loi1"] = "Chưa nhập tài khoản"; 
            }
            else if(String.IsNullOrEmpty(matkhau))
            {
                ViewData["loi2"] = "Bạn Chưa Nhập Mật Khẩu";
            }
            else
            { 
                QUANLY ql = db.QUANLies.SingleOrDefault(n => n.USERNAME == ten && n.PASS == matkhau);
                if (ql != null)
                {
                    SetAlert("Đăng nhập thành công!", "success");
                    Session["Admin"] = ql;
                    return RedirectToAction("ShowSanpham", "Admin");

                }
                else
                    SetAlert("Đăng nhập sai!", "error");
            }
            return View();
        }
        /*** San pham ***/
        [HttpGet]
        public ActionResult themsp()
        {
            ViewBag.MALOAI = new SelectList(db.LOAISPs.OrderBy(n => n.MALOAI), "MALOAI", "TENLOAI");
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult themsp(SANPHAM sp, HttpPostedFileBase fileUpload)
        {
            ViewBag.MALOAI = new SelectList(db.LOAISPs.OrderBy(n => n.MALOAI), "MALOAI", "TENLOAI");

            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            
            else
            {
                if (ModelState.IsValid)
                {
                    
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    
                    var path = Path.Combine(Server.MapPath("~/image/"), fileName);
                  
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                    }
                    sp.ANH = fileName;
                    sp.NGAYNHAP = DateTime.Now;
                    db.SANPHAMs.InsertOnSubmit(sp);
                    db.SubmitChanges();
                }
                return View();
            }
        }
        
        public ActionResult ShowSanpham(int ? page)
        {
            int pageSize = 7;
            int pageNum = (page ?? 1);
            var a = from tt in db.SANPHAMs select tt  ;
            return View(a.ToPagedList(pageNum,pageSize));
        }
        /*** Loai San Pham ***/
        [HttpGet]
        public ActionResult themloai()
        {
            ViewBag.MADM = new SelectList(db.DANHMUCs.OrderBy(n => n.MADM), "MADM", "TENDANHMUC");
            return View();
            
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult themloai(LOAISP l)
        {
            ViewBag.MADM = new SelectList(db.DANHMUCs.OrderBy(n => n.MADM), "MADM", "TENDANHMUC");
       
            db.LOAISPs.InsertOnSubmit(l);
            db.SubmitChanges();
            return View();
        }
        /**** Danh Mục *****/
        [HttpGet]
        public ActionResult themdanhmuc()
        {
            
            return View();

        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult themdanhmuc(DANHMUC D)
        {

            db.DANHMUCs.InsertOnSubmit(D);
            db.SubmitChanges();
            return View();
        }
        public ActionResult Showdm(int? page)
        {
            int pageSize = 10;
            int pageNum = (page ?? 1);
            var a = from tt in db.DANHMUCs select tt;
            return View(a.ToPagedList(pageNum, pageSize));
        }
        [HttpGet]
        public ActionResult DeleteDM(int id)
        {
            var d = db.DANHMUCs.First(n => n.MADM == id);
            return View(d);
        }
        [HttpPost]
        public ActionResult DeleteDM(int id, FormCollection col)
        {
            var d = db.DANHMUCs.First(n => n.MADM == id);
            db.DANHMUCs.DeleteOnSubmit(d);
            db.SubmitChanges();
            return RedirectToAction("Showdm", "Admin");
        }
        /****/
        [HttpGet]
        public ActionResult EditSP(int id)
        {
            ViewBag.MALOAI = new SelectList(db.LOAISPs.OrderBy(n => n.MALOAI), "MALOAI", "TENLOAI");
            var sua = db.SANPHAMs.First(n => n.MASP == id);
                return View(sua);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult EditSP(int id , HttpPostedFileBase fileUpload,SANPHAM sp)
        {
            ViewBag.MALOAI = new SelectList(db.LOAISPs.OrderBy(n => n.MALOAI), "MALOAI", "TENLOAI");
            var sua = db.SANPHAMs.First(n => n.MASP == id);


            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }

            else
            {
                if (ModelState.IsValid)
                {

                    var fileName = Path.GetFileName(fileUpload.FileName);

                    var path = Path.Combine(Server.MapPath("~/image"), fileName);

                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
              
                    }
                    else
                    {
                        //Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                    }
                    sua.ANH = fileName;
                    sua.MASP = id;
                    UpdateModel(sua);
                    db.SubmitChanges();
                }

                return this.EditSP(id);
            }
        }
        public ActionResult Details(int id)
        {
            var dt = db.SANPHAMs.Where(n => n.MASP == id).First();
            return View(dt);
        }
        public ActionResult Delete(int id)
        {
            var sp = db.SANPHAMs.First(n => n.MASP == id);
            return View(sp);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection col)
        {
            var sp = db.SANPHAMs.First(n => n.MASP == id);
            db.SANPHAMs.DeleteOnSubmit(sp);
            db.SubmitChanges();
            return RedirectToAction("ShowSanpham", "Admin");
        }
        /**** ShowLoai/Edit/Delete ***/
        public ActionResult Showloai(int? page)
        {
            int pageSize = 10;
            int pageNum = (page ?? 1);
            var a = from tt in db.LOAISPs select tt;
            return View(a.ToPagedList(pageNum, pageSize));
        }
        [HttpGet]
        public ActionResult EditL(int id)
        {
            var loai = db.LOAISPs.First(n => n.MALOAI == id);
            return View(loai);
        }
        [HttpPost]
        public ActionResult EditL(int id, FormCollection collection)
        {
            var loai = db.LOAISPs.First(n => n.MALOAI == id);

            loai.MALOAI = id;
            UpdateModel(loai);
            db.SubmitChanges();
            return this.EditL(id);
        }
        [HttpGet]
        public ActionResult DeleteL(int id)
        {
            var loai = db.LOAISPs.First(n => n.MALOAI == id);
            return View(loai);
        }
        [HttpPost]
        public ActionResult DeleteL(int id,FormCollection col)
        {
            var loai = db.LOAISPs.First(n => n.MALOAI == id);
            db.LOAISPs.DeleteOnSubmit(loai);
            db.SubmitChanges();
            return RedirectToAction("Showloai", "Admin");
        }
        /***Don dat hang***/
        public ActionResult ShowCTDH(int? page)
        {
            int pageSize = 10;
            int pageNum = (page ?? 1);
            var a = from tt in db.CTDATHANGs select tt;
            return View(a.ToPagedList(pageNum, pageSize));
        }
        [HttpGet]
        public ActionResult EditDDH(int id)
        {
            var ddh = db.CTDATHANGs.First(n => n.MACT == id);
            return View(ddh);
        }
        [HttpPost]
        public ActionResult EditDDH(int id, FormCollection collection)
        {
            var ddh = db.CTDATHANGs.First(n => n.MACT == id);
            ddh.MACT = id;
            UpdateModel(ddh);
            db.SubmitChanges();
            return this.EditDDH(id);
        }
        public ActionResult DetailsDDH(int id)
        {
            var dt = db.CTDATHANGs.Where(n => n.MACT == id).First();
            return View(dt);
        }
        [HttpGet]
        public ActionResult DeleteDDH(int id)
        {
            var loai = db.CTDATHANGs.First(n => n.MACT == id);
            return View(loai);
        }
        [HttpPost]
        public ActionResult DeleteDDH(int id, FormCollection col)
        {
            var loai = db.CTDATHANGs.First(n => n.MACT == id);
            db.CTDATHANGs.DeleteOnSubmit(loai);
            db.SubmitChanges();
            return RedirectToAction("ShowCTDH", "Admin");
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        public ActionResult DangKy(FormCollection collection, QUANLY ql)
        {
            
            var taikhoan = collection["id"];
            var matkhau = collection["pass"];
            var sdt = collection["phone"];
            var hoten = collection["username"];
            ql.USERNAME = taikhoan;
            ql.HOTEN = hoten;
            ql.SDT = sdt;
            ql.PASS = matkhau;
            db.QUANLies.InsertOnSubmit(ql);
            db.SubmitChanges();
            return RedirectToAction("Index", "Admin");
        }
        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if(type=="success")
            {
                TempData["AlertType"]="alert-success";

            }
            else if(type=="warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if(type=="error")
            {
                TempData["AlertType"] = "alert-danger";
            }

        }
        


    }
}