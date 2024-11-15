using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Web.UI.WebControls;
using Microsoft.Web.Helpers;
using System.Data;
using System.Data.Entity.Migrations;

namespace BookStore.Controllers
{
    public class AdminController : Controller
    {
        private readonly QLBANSACHEntities _context;

        // Constructor
        public AdminController()
        {
            _context = new QLBANSACHEntities();
        }
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            // Gán các giá trị người dùng nhập liệu cho các biến
            var tendn = collection["username"];
            var matkhau = collection["password"];

            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                // Gán giá trị cho đối tượng được tạo mới (ad)
                admin ad = _context.admin.SingleOrDefault(n => n.usernameAd == tendn && n.passwordAd == matkhau);
                if (ad != null)
                {
                    ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                    Session["TaikhoanAdmin"] = ad;
                    // Kiểm tra xem Session có lưu được không
                    if (Session["TaikhoanAdmin"] != null)
                    {
                        return RedirectToAction("Sach");
                    }
                    else
                    {
                        ViewBag.Thongbao = "Không thể lưu Session.";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
                    return View();
                }
            }
            return View();
        }

        public ActionResult Sach(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return View(_context.SACH.ToList().OrderBy(n =>n.Masach).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult themSach()
        {
            // Dua du lieu vao dropdownList
            // Lay ds tu table chu de, sap xep tang dan theo ten chu de, chon lay gia tri Ma CD, hien thi thi TenChude
            ViewBag.MaCD = new SelectList(_context.CHUDE.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(_context.NHAXUATBAN.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult themSach(SACH sach, HttpPostedFileBase fileUpload)
        {
            // Dua du lieu vao dropdownList
            // Lay ds tu table chu de, sap xep tang dan theo ten chu de, chon lay gia tri Ma CD, hien thi thi TenChude
            ViewBag.MaCD = new SelectList(_context.CHUDE.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(_context.NHAXUATBAN.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");


            if (fileUpload == null) 
            {
                ViewBag.Thongbao = "Chon anh bia";
            }

            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    // Lưu đường dẫn của file
                    var path = Path.Combine(Server.MapPath("~/image"), fileName);
                    // Kiểm tra hình ảnh tồn tại chưa?
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        return View();
                    }
                    else
                    {
                        // Lưu hình ảnh vào đường dẫn
                        fileUpload.SaveAs(path);
                    }
                    sach.Anhbia = fileName;
                    _context.SACH.Add(sach);
                    _context.SaveChanges();
                    return RedirectToAction("Sach");
                }
                return View(sach);
            }

            return View(sach);
        }

        public ActionResult details(int id)
        {
            SACH sach = _context.SACH.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if(sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        [HttpGet]
        public ActionResult delete(int id)
        {
            SACH sach = _context.SACH.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        [HttpPost, ActionName("delete")]
        public ActionResult xacnhanXoa(int id)
        {
            SACH sach = _context.SACH.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            _context.SACH.Remove(sach);
            _context.SaveChanges();
            return RedirectToAction("Sach");
        }

        [HttpGet]
        public ActionResult edit(int id)
        {
            SACH sach = _context.SACH.SingleOrDefault(n => n.Masach == id);

            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaCD = new SelectList(_context.CHUDE.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", sach.MaCD);
            ViewBag.MaNXB = new SelectList(_context.NHAXUATBAN.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);
            return View(sach);
        }

        [HttpPost]
        public ActionResult edit(SACH sach, HttpPostedFileBase fileUpload)
        {
            // Dữ liệu dropdown
            ViewBag.MaCD = new SelectList(_context.CHUDE.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(_context.NHAXUATBAN.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");

            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                // Kiểm tra tính hợp lệ của file
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/image"), fileName);

                    // Kiểm tra xem file đã tồn tại chưa
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        return View(sach); // Trả về trang chỉnh sửa nếu file đã tồn tại
                    }
                    else
                    {
                        // Lưu file vào thư mục
                        fileUpload.SaveAs(path);
                        sach.Anhbia = fileName;  // Cập nhật ảnh mới
                    }
                }
            }
            else
            {
                // Nếu không có file mới, giữ lại ảnh cũ
                sach.Anhbia = sach.Anhbia;
            }

            if (ModelState.IsValid)
            {
                _context.SACH.AddOrUpdate(sach);
                _context.SaveChanges();
                return RedirectToAction("Sach");
            }
            return View(sach);
        }

        public ActionResult ThongKeSach()
        {
            var bookStatistics = _context.SACH
           .GroupBy(s => s.CHUDE.TenChuDe)
           .ToDictionary(
               g => g.Key?.ToString() ?? "Unknown", // Convert nullable int to string, handle null as "Unknown"
               g => g.Count()
           );

            return View(bookStatistics); // Pass the dictionary as a model to the view
        }

    }
}