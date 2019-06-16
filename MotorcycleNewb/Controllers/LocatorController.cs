using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using MotorcycleNewb.ServiceLayer;
using MotorcycleNewb.Models.DataAccessLayer;
using MotorcycleNewb.Models;

namespace MotorcycleNewb.Controllers
{
    public class LocatorController : Controller
    {
        private ApplicationServices applicationServices = new ApplicationServices(new UnitOfWork(new ApplicationDbContext()));

        // GET: Locator Map
        public ActionResult Index()
        {
            string markers = "[";
            string conString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlCommand cmd = new SqlCommand("SELECT * FROM Locations");
            using (SqlConnection con = new SqlConnection(conString))
            {
                cmd.Connection = con;
                con.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        markers += "{";
                        markers += string.Format("'title': '{0}',", sdr["Name"]);
                        markers += string.Format("'lat': '{0}',", sdr["Latitude"]);
                        markers += string.Format("'lng': '{0}',", sdr["Longitude"]);
                        markers += string.Format("'description': '{0}'", sdr["Description"]);
                        markers += "},";
                    }
                }
                con.Close();
            }

            markers += "];";
            ViewBag.Markers = markers;

            // Requried in order to avoid "Microsoft.CSharp.RuntimeBinder.RuntimeBinderException: 'Cannot perform runtime binding on a null reference'" error...
            var accountId = applicationServices.GetCurrentAccountId(User);
            Profile profile = applicationServices.GetProfile(accountId);

            ProfileViewModel user = new ProfileViewModel()
            {
                CurrAccId = accountId, //Test
                CurrentProfile = profile,
                IsThisUser = applicationServices.EnsureIsUserProfile(profile, User)
            };


            return View(user);
        }
    }
}