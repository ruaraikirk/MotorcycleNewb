using MotorcycleNewb.Models;
using MotorcycleNewb.Models.DataAccessLayer;
using MotorcycleNewb.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotorcycleNewb.Controllers
{
    public class WallController : Controller
    {
        private ApplicationServices applicationServices = new ApplicationServices(new UnitOfWork(new ApplicationDbContext()));

        // GET: Wall
        public ActionResult Index()
        {
            // Profile page (wall) after the login or sign up
            // The page has profile personal details, etc.

            var accountId = applicationServices.GetCurrentAccountId(User);
            Profile profile = applicationServices.GetProfile(accountId);

            ProfileViewModel wall = new ProfileViewModel()
            {
                CurrAccId = accountId, //Test
                CurrentProfile = profile,
                IsThisUser = applicationServices.EnsureIsUserProfile(profile, User)
            };

            return View(wall);
        }
    }
}