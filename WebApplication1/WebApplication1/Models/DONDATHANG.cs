namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DONDATHANG")]
    public partial class DONDATHANG
    {
        [Key]
        public int MaDonHang { get; set; }

        public bool? Dathanhtoan { get; set; }

        public bool? Tinhtranggiaohang { get; set; }

        public DateTime? Ngaydat { get; set; }

        public DateTime? Ngaygiao { get; set; }

        public int? MaKH { get; set; }

        public virtual KHACHHANG KHACHHANG { get; set; }
    }
}
