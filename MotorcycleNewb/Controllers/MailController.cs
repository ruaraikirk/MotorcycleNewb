using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MotorcycleNewb.Models;
using MotorcycleNewb.Models.DataAccessLayer;
using MotorcycleNewb.ServiceLayer;

namespace MotorcycleNewb.Controllers
{
    public class MailController : Controller
    {
        private ApplicationServices applicationServices;

        // Constructors 
        public MailController()
        {
            applicationServices = new ApplicationServices(new UnitOfWork(new ApplicationDbContext()));
        }
        public MailController(IUnitOfWork unit)
        {
            applicationServices = new ApplicationServices(unit);
        }
        /*
        // GET: Mail
        public ActionResult Index()
        {
            return View(applicationServices.GetMails());
        }
        */
        /*
        // GET: Mail/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mail mail = applicationServices.Mails.Find(id);
            if (mail == null)
            {
                return HttpNotFound();
            }
            return View(mail);
        }
        */
        // GET: Mail/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Mail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Email,FirstName")] Mail mail)
        {
            if (ModelState.IsValid)
            {
                applicationServices.Add(mail);
                applicationServices.Save();
                return RedirectToAction("Index", "Home");
            }

            return View(mail);
        }
        
        /*
        // GET: Mail/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mail mail = applicationServices.Mails.Find(id);
            if (mail == null)
            {
                return HttpNotFound();
            }
            return View(mail);
        }
        */
        /*
        // POST: Mail/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Email,FirstName")] Mail mail)
        {
            if (ModelState.IsValid)
            {
                applicationServices.Entry(mail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mail);
        }
        */
        /*
        // GET: Mail/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mail mail = db.Mails.Find(id);
            if (mail == null)
            {
                return HttpNotFound();
            }
            return View(mail);
        }
        */
        /*
        // POST: Mail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Mail mail = db.Mails.Find(id);
            db.Mails.Remove(mail);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        */
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                applicationServices.DisposeContext();
            }
            base.Dispose(disposing);
        }
    }
}
