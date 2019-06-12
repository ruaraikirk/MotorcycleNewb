using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
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
         */

        
        // GET: Profiles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "An error occurred whilst processing your request.");
            }
            Profile profile = applicationServices.GetProfile(id);
            bool isUser = applicationServices.EnsureIsUserProfile(profile, User);
            if (profile == null || !isUser)
            {
                return HttpNotFound();
            }

            ProfileViewModel vm = new ProfileViewModel
            {
                CurrentProfile = profile,
                CurrentView = "profile",
                IsThisUser = isUser,
                ProfilePhoto = profile.ProfileImage.FileName,
                MotorcyclePhoto = profile.MotorcycleImage.FileName
            };

            ViewBag.Partial = "Edit";

            ViewBag.ApplicationId = applicationServices.GetCurrentAccountId(User);
            return View(vm);
        }

        // POST: Profiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CurrentProfile")] ProfileViewModel vm)
        {
            ViewBag.AccountId = applicationServices.GetCurrentAccountId(User);
            if (ModelState.IsValid)
            {
                applicationServices.Edit(vm.CurrentProfile);
                applicationServices.Save();
                return RedirectToAction("Index", "Wall");

            }
            return RedirectToAction("Edit", new { id = vm.CurrentProfile.ProfileId });
        }

        // *Profile* image edit
        // GET: Profiles/ChangeProfileImage/5
        public ActionResult ChangeProfileImage(int? id)
        {
            //Change the profile photo
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "An error occurred whilst processing your request!");
            }

            var accountId = applicationServices.GetCurrentAccountId(User);
            Profile profile = applicationServices.GetProfile(accountId);
            bool isUser = applicationServices.EnsureIsUserProfile(profile, User);

            if (profile == null || !isUser)
            {
                return HttpNotFound();
            }

            ProfileViewModel vm = new ProfileViewModel
            {
                CurrentProfile = profile,
                CurrentView = "profile",
                IsThisUser = isUser,
                ProfilePhoto = profile.ProfileImage.FileName,

            };

            return View("ChangeProfileImage", vm);
        }

        // POST: Profiles/ChangeProfileImage/5
        [HttpPost]
        public ActionResult ChangeProfileImage(HttpPostedFileBase upload)
        {
            //Save new image into database from the file uploaded
            Profile profile = applicationServices.GetProfile(applicationServices.GetCurrentAccountId(User));
            Image image = profile.ProfileImage;
            try
            {
                if (ModelState.IsValid)
                {
                    if (upload != null && upload.ContentLength > 0)
                    {

                        using (var reader = new BinaryReader(upload.InputStream))
                        {
                            image.Content = reader.ReadBytes(upload.ContentLength);

                        }
                        applicationServices.Edit(profile);
                        applicationServices.Save();

                    }
                    return RedirectToAction("Index", "Wall");
                    //return RedirectToAction("MyProfile", new { id = profile.ProfileId });
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            ProfileViewModel vm = new ProfileViewModel
            {
                CurrentProfile = profile,
                CurrentView = "profile",
                IsThisUser = applicationServices.EnsureIsUserProfile(profile, User),
                ProfilePhoto = profile.ProfileImage.FileName
            };

            return View("ChangeProfileImage", vm);
        }

        // *Motorcycle* image edit (duplicating code here, can this be refactored if time?
        // GET: Profiles/ChangeMotorcycleImage/5
        public ActionResult ChangeMotorcycleImage(int? id)
        {
            //Change the users motorcycle photo
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "An error occurred whilst processing your request!");
            }

            var accountId = applicationServices.GetCurrentAccountId(User);
            Profile profile = applicationServices.GetProfile(accountId);
            bool isUser = applicationServices.EnsureIsUserProfile(profile, User);

            if (profile == null || !isUser)
            {
                return HttpNotFound();
            }

            ProfileViewModel vm = new ProfileViewModel
            {
                CurrentProfile = profile,
                CurrentView = "profile",
                IsThisUser = isUser,
                MotorcyclePhoto = profile.MotorcycleImage.FileName,

            };

            return View("ChangeMotorcycleImage", vm);
        }

        // POST: Profiles/ChangeMotorcycleImage/5
        [HttpPost]
        public ActionResult ChangeMotorcycleImage(HttpPostedFileBase upload)
        {
            //Save new image into database from the file uploaded
            Profile profile = applicationServices.GetProfile(applicationServices.GetCurrentAccountId(User));
            Image image = profile.MotorcycleImage;
            try
            {
                if (ModelState.IsValid)
                {
                    if (upload != null && upload.ContentLength > 0)
                    {

                        using (var reader = new BinaryReader(upload.InputStream))
                        {
                            image.Content = reader.ReadBytes(upload.ContentLength);

                        }
                        applicationServices.Edit(profile);
                        applicationServices.Save();

                    }
                    return RedirectToAction("Index", "Wall");
                    //return RedirectToAction("MyProfile", new { id = profile.ProfileId });
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            ProfileViewModel vm = new ProfileViewModel
            {
                CurrentProfile = profile,
                CurrentView = "profile",
                IsThisUser = applicationServices.EnsureIsUserProfile(profile, User),
                MotorcyclePhoto = profile.MotorcycleImage.FileName
            };

            return View("ChangeMotorcycleImage", vm);
        }
        // END Motorcycle image edit
        
        // GET: Profiles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile profile = applicationServices.GetProfile(id);  // TODO update to ProfileViewModel
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
            Profile profile = applicationServices.GetProfile(id); // TODO update to ProfileViewModel
            applicationServices.Remove(profile);
            applicationServices.Save();

            // TODO Deleting from ASP.NET Identity
            return RedirectToAction("LogOff", "Account"); // TOTO Fix Redirect to log user out and return to home page.
            // See https://stackoverflow.com/questions/24318341/how-to-log-off-from-another-action for examples.
        }

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
