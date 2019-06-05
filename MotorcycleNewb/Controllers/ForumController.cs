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

        public ActionResult Lurker()
        {

            IEnumerable<Post> posts = db.GetPosts().OrderByDescending(p => p.Timestamp);

            return View(posts);
        }

        public ActionResult Member(int? id)
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
            /*
            //If the profile examined is not the current user then change some properties
            if (!wvm.IsThisUser)
            {
                var user = appService.GetProfile(appService.GetCurrentUserId(User));

                wvm.FriendStatus = appService.GetFriendStatus(user, profile).Item1;
                wvm.ButtonStatus = appService.GetFriendStatus(user, profile).Item2;
                wvm.User = user;
                wvm.Photo = user.ProfilePic.FileName;
            }
            */
            return View(vm);
        }

        /*POSTS*/
        [HttpPost]
        //[Route("Forum/UploadPost")]
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
            /*
            //If the post is uploaded into activity, stay in the activity page
            if (page.Equals("Forum"))
            {
                return RedirectToAction("Forum", new { id = post.ProfileId });
            }*/
            return RedirectToAction("Member", new { id = post.ProfileId });
        }

        [HttpPost]
        //[Route("Forum/UploadComment")]
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
            /*if (page.Equals("Forum"))
            {
                return RedirectToAction("Activity", new { id = comment.ProfileId });
            }*/
            return RedirectToAction("Member", new { id = comment.ProfileId });
        }
    }
}