using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTBGD.Models
{
    public class GioHang
    {
        DBTHIETBIGIADUNGDataContext db = new DBTHIETBIGIADUNGDataContext();
        public int iMASP { set; get; }
        public string iTENSP { set; get; }
        public string iANH { set; get; }
        public Double iGIA { set; get; }
        public int iSOLUONG { set; get; }
        public Double THANHTIEN
        {
            get { return iSOLUONG * iGIA; }
        }
        public GioHang(int MA)
        {
            iMASP = MA;
            SANPHAM sp = db.SANPHAMs.Single(n => n.MASP == iMASP);
            iTENSP = sp.TENSP;
            iANH = sp.ANH;
            iGIA = double.Parse(sp.GIA.ToString());
            iSOLUONG = 1;


        }
    }
}