using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PagedList;
using PagedList.Mvc;
using System.Web.UI;

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
        public ActionResult Index(int? page)
        {
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            var sach = _context.SACH.OrderBy(s => s.Masach).ToPagedList(pageNumber, pageSize);

            return View(sach);
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