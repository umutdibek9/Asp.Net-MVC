using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Yeni.Models.DataContext;

namespace Yeni.Controllers
{
    public class BlogController : Controller
    {
        private KurumsalDBContext db = new KurumsalDBContext();
        // GET: Blog
        public ActionResult Index()
        {
            return View(db.Blog.ToList());
        }
    }
}