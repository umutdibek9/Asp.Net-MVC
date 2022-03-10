using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Yeni.Models.DataContext;

namespace Yeni.Controllers
{
    public class HomeController : Controller
    {
        private KurumsalDBContext db = new KurumsalDBContext();
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            ViewBag.Hizmetler = db.Hizmet.ToList().OrderByDescending(x => x.HizmetId);
            return View();
        }
        public ActionResult SliderPartial()
        {
            return View(db.Slider.ToList().OrderByDescending(x=>x.SliderId));
        }
        public ActionResult HizmetPartial()
        {
            return View(db.Hizmet.ToList());
        }
        public ActionResult Hakkimizda()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Hakkimizda.SingleOrDefault());
        }
        public ActionResult Hizmetlerimiz()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Hizmet.ToList().OrderByDescending(x=>x.HizmetId));
        }
        public ActionResult Iletisim()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Iletisim.SingleOrDefault());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Iletisim(string adsoyad=null,string email=null,string konu=null,string mesaj=null)
        {
            if(adsoyad!=null && email != null)
            {
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "fbumut236@gmail.com";
                WebMail.Password = "123yeni123";
                WebMail.SmtpPort = 587;
                WebMail.Send("fbumut236@gmail.com", konu, email + "</br>" + mesaj);
                ViewBag.Uyari = "Mesajınız başarı ile gönderilmiştir";
                Response.Redirect("/Home/Iletisim/");
            }
            else
            {
                ViewBag.Uyari = "Hata oluştu.Tekrar deneyiniz";
            }
            return View();
        }
        public ActionResult FooterPartial()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            ViewBag.Hizmetler = db.Hizmet.ToList().OrderByDescending(x => x.HizmetId);
            ViewBag.Iletisim = db.Iletisim.SingleOrDefault();
            return PartialView();
        }
    }
}