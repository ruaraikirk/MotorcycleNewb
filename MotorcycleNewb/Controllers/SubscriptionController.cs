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
    public class SubscriptionController : Controller
    {
        private ApplicationServices db = new ApplicationServices(new UnitOfWork(new ApplicationDbContext()));
        /*
        // GET: Subscription
        public ActionResult Index()
        {
            return View(db.Subscriptions.ToList());
        }
        */
        // GET: Subscription/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Subscription/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubscriptionID, Name, Email")]Subscription subscription)
        //public ActionResult Create(Subscription subscription)
        {
            if (ModelState.IsValid)
            {
                db.Add(subscription);
                db.Save();
                return RedirectToAction("Index", "Home");
            }

            return View(subscription);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.DisposeContext();
            }
            base.Dispose(disposing);
        }
    }
}
