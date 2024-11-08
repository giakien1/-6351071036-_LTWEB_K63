using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class Giohang
    {
        private readonly QLBANSACHEntities _context;

        // Constructor
        public Giohang()
        {
            _context = new QLBANSACHEntities();
        }

        public int iMasach { get; set; }
        public string sTensach { get; set; }
        public string sAnhbia { get; set; }
        public double dDongia { get; set; }
        public int iSoluong { get; set; }

        public double dThanhtien
        {
            get { return iSoluong * dDongia; }
        }

        // Khởi tạo giỏ hàng theo Masach được truyền vào với iSoluong mặc định là 1.
        public Giohang(int Masach)
        {
            _context = new QLBANSACHEntities();
            iMasach = Masach;
            SACH sach = _context.SACH.Single(n => n.Masach == iMasach);
            sTensach = sach.Tensach;
            sAnhbia = sach.Anhbia;
            dDongia = double.Parse(sach.Giaban.ToString());
            iSoluong = 1;
        }
    }
}