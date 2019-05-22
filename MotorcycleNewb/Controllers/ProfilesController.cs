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
    public class ProfilesController : Controller
    {
        // private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationServices applicationServices;

        // Constructors 
        public ProfilesController()
        {
            applicationServices = new ApplicationServices(new UnitOfWork(new ApplicationDbContext()));
        }
        public ProfilesController(IUnitOfWork unit)
        {
            applicationServices = new ApplicationServices(unit);
        }

        // GET: Profiles
        public ActionResult Index()
        {
            return View();
        }

        // Action to launch my profile page where edit, view profile deatils...
        // NOT DONE, 
        /*
         * STUB
         */ 

        // GET: Profiles/Details/5 ***See comments for changes, needs review
        public ActionResult Details(int? id) // Test
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Oops! An error occurred whilst processing your request.");
            }
            Profile profile = applicationServices.GetProfile(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            ProfileViewModel profileViewModel = new ProfileViewModel
            {
                CurrentProfile = profile,
                IsThisUser = applicationServices.EnsureIsUserProfile(profile, User),
                CurrentView = "profile",
                ProfilePhoto = profile.ProfileImage.FileName,
                MotorcyclePhoto = profile.MotorcycleImage.FileName
            };

            ViewBag.Partial = "Details"; //To check if it's edit or view section in Profile section of My profile page

            return View(profileViewModel); // Changed
        }

        // GET: Profiles/Create
        public ActionResult Create()
        {
            ViewBag.AccountId = applicationServices.GetCurrentAccountId(User);

            return View();
        }

        // POST: Profiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Profile profile)
        {
            if (ModelState.IsValid)
            {
                ViewBag.AccountId = applicationServices.GetCurrentAccountId(User);
                Image profilePic = new Image { FileName = "../../Content/images/helmet-avatar.png" };
                profile.ProfileImage = profilePic;
                Image mcPic = new Image { FileName = "../../Content/images/motorcycle-image-avatar.png" };
                profile.MotorcycleImage = mcPic;
                applicationServices.Add(profile);
                applicationServices.Save();
                return RedirectToAction("Index", "Wall"); // Redirect to Wall (profile page) following profile creation
            }
            //ViewBag.AccountId = applicationServices.GetCurrentAccountId(User); // Delete in final clean up...
            return View(profile);
        }

        /*
         * TODO - Complete CRUD operations for profile details...
         * TODO - Add Change and upload image operations...
         */

        /*
        // GET: Profiles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }

        // POST: Profiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProfileId,FirstName,LastName,City")] Profile profile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(profile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(profile);
        }

        // GET: Profiles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }

        // POST: Profiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Profile profile = db.Profiles.Find(id);
            db.Profiles.Remove(profile);
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
