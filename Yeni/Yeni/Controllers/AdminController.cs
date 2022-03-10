using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Yeni.Models;
using Yeni.Models.DataContext;
using Yeni.Models.Model;

namespace Yeni.Controllers
{
    public class AdminController : Controller
    {
        KurumsalDBContext db = new KurumsalDBContext();
        public ActionResult Index()
        {
            var sorgu = db.Kategori.ToList();
            return View(sorgu);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Admin admin)
        {
            var login = db.Admin.Where(x => x.Eposta == admin.Eposta).SingleOrDefault();
            if (login.Eposta == admin.Eposta && login.Sifre == Crypto.Hash(admin.Sifre,"MD5"))
            {
                Session["adminid"] = login.AdminId;
                Session["eposta"] = login.Eposta;
                Session["yetki"] = login.Yetki;
                return RedirectToAction("Index", "Admin");
            }
            ViewBag.Uyari = "Kullanici adi yada şifre hatali";
            
            return View(admin);
        }
        public ActionResult Logout()
        {
            Session["adminid"] = null;
            Session["eposta"] = null;
            Session.Abandon();
            return RedirectToAction("Login", "Admin");
        }
        public ActionResult SifremiUnuttum()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SifremiUnuttum(string eposta)
        {
            var mail = db.Admin.Where(x => x.Eposta == eposta).SingleOrDefault();
            if (mail!=null)
            {
                Random rnd = new Random();
                int yenisifre = rnd.Next();
                Admin sifre = new Admin();
                mail.Sifre = Crypto.Hash(Convert.ToString(yenisifre), "MD5");
                db.SaveChanges();
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "fbumut236@gmail.com";
                WebMail.Password = "123yeni123";
                WebMail.SmtpPort = 587;
                WebMail.Send(eposta,"Admin Panel Giriş Şifreniz", "Şifreniz :" + yenisifre);
                ViewBag.Uyari = "Mesajınız başarı ile gönderilmiştir";
            }
            else
            {
                ViewBag.Uyari = "Hata oluştu.Tekrar deneyiniz";
            }
            return View();
        }
        public ActionResult Adminler()
        {
            return View(db.Admin.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Admin admin,string sifre,string eposta)
        {
            if (ModelState.IsValid)
            {
                admin.Sifre = Crypto.Hash(sifre, "MD5");
                db.Admin.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Adminler");
            }
            return View(admin);
        }
        public ActionResult Edit(int id)
        {
            var a = db.Admin.Where(x => x.AdminId == id).SingleOrDefault();
            return View(a);
        }
        [HttpPost]
        public ActionResult Edit(int id,Admin admin,string sifre,string eposta)
        {
            if (ModelState.IsValid)
            {
                var a = db.Admin.Where(x => x.AdminId == id).SingleOrDefault();
                a.Sifre = Crypto.Hash(sifre, "MD5");
                a.Eposta = admin.Eposta;
                a.Yetki = admin.Yetki;
                db.SaveChanges();
                return RedirectToAction("Adminler");
            }
            return View(admin);
        }
        public ActionResult Delete(int id)
        {
            var a = db.Admin.Where(x => x.AdminId == id).SingleOrDefault();
            if (a != null)
            {
                db.Admin.Remove(a);
                db.SaveChanges();
                return RedirectToAction("Adminler");
            }
            return View();
        }
    }
}