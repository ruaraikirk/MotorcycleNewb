using MotorcycleNewb.Models;
using MotorcycleNewb.Models.DataAccessLayer;
using MotorcycleNewb.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MotorcycleNewb.Controllers
{
    public class ForumController : Controller
    {

        private ApplicationServices db = new ApplicationServices(new UnitOfWork(new ApplicationDbContext()));

        public ActionResult Lurker() // For unregistered users to view but not take part in the forum
        {

            IEnumerable<Post> posts = db.GetPosts().OrderByDescending(p => p.Timestamp);

            return View(posts);
        }

        public ActionResult Member(int? id) // Registered user access to forum
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile profile = db.GetProfile(id);
            if (profile == null)
            {
                return HttpNotFound();
            }

            // Get a collections of all posts
            IEnumerable<Post> posts = db.GetPosts().OrderByDescending(p => p.Timestamp);

            // Instantiate a ProfileViewModel with some properties
            ProfileViewModel vm = new ProfileViewModel()
            {
                CurrentProfile = profile,
                Posts = posts,
                CurrentView = "Member",
                IsThisUser = db.EnsureIsUserProfile(profile, User),
                ProfilePhoto = profile.ProfileImage.FileName
            };

            return View(vm);
        }

        /*
         * POSTS
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadPost([Bind(Include = "PostId, PostContent, Timestamp, ProfileId")]Post post, string page)
        {

            post.ProfileId = db.GetProfile(db.GetCurrentAccountId(User)).ProfileId;
            post.Timestamp = DateTime.Now;

            if (ModelState.IsValid && !string.IsNullOrWhiteSpace(post.PostContent))
            {
                db.Add(post);
                db.Save();
            }

            return RedirectToAction("Member", new { id = post.ProfileId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadComment([Bind(Include = "CommentId, CommentContent, Timestamp, ProfileId, PostId")]Comment comment, string page)
        {

            comment.ProfileId = db.GetProfile(db.GetCurrentAccountId(User)).ProfileId;
            comment.Timestamp = DateTime.Now;

            if (ModelState.IsValid && !string.IsNullOrWhiteSpace(comment.CommentContent))
            {
                db.Add(comment);
                db.Save();
            }

            return RedirectToAction("Member", new { id = comment.ProfileId });
        }
    }
}