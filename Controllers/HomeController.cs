using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Models
{
   
    public class HomeController : Controller
    {
        private readonly QLBANSACHEntities _context;

        // Constructor
        public HomeController()
        {
            _context = new QLBANSACHEntities();
        }

        // GET: Home
        public ActionResult Index()
        {
            var newestBook = _context.SACH.OrderByDescending(s => s.Ngaycapnhat).Take(5).ToList();
            return View(newestBook);
        }

        public ActionResult Categories()
        {
            var cate = _context.CHUDE.ToList();
            return PartialView(cate);
        }

        public ActionResult Publisher()
        {
            var pub = _context.NHAXUATBAN.ToList();
            return PartialView(pub);
        }

        public ActionResult itemInCate(int id) 
        {
            var book = _context.SACH.Where(s => s.MaCD == id).ToList();
            return View(book);
        }

        public ActionResult itemPub(int id)
        {
            var pub = _context.SACH.Where(s => s.MaNXB == id).ToList();
            return View(pub);
        }

        public ActionResult Details(int id)
        {
            var book = _context.SACH.Where(s => s.Masach == id).ToList();
            return View(book.Single());
        }

        //[HttpPost]
        //public ActionResult Index()
        //{
        //    return View();
        //}
    }
}