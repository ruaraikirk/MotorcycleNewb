﻿using Microsoft.AspNet.Identity;
using MotorcycleNewb.Models;
using MotorcycleNewb.Models.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace MotorcycleNewb.ServiceLayer
{
    public class ApplicationServices
    {
        private readonly IUnitOfWork unit;

        public ApplicationServices(IUnitOfWork unit)
        {
            this.unit = unit;
        }
        public void Save()
        {
            unit.Commit();
        }
        public void DisposeContext()
        {
            unit.Dispose();
        }

        /*
         * Profile CRUD logic/operations 
         */

        // Add new profile to database
        public void Add(Profile profile)
        {
            unit.Profiles.Add(profile);
        }
        // Modify an existing entity into database
        public void Edit(Profile profile)
        {
            unit.Update(profile);
        }
        // Remove entity from database
        public void Remove(Profile profile)
        {
            unit.Delete(profile);
        }

        /*
         * Supporting PROFILE methods 
         */

        //Get profile object from a given AcountId (dbo.AspNetUsers.Id) ****
        public Profile GetProfile(Guid id)
        {
            return unit.Profiles.GetElement(x => x.AccountId == id);
        }
        // Get profile object from a given ProfileId
        public Profile GetProfile(int? id)
        {
            return unit.Profiles.GetElement(x => x.ProfileId == id);
        }
        // Get the ProfileId from AcountId
        public int GetProfileId(Guid guid)
        {
            return GetProfile(guid).ProfileId;
        }
        // Get the AcountId of the current user
        public Guid GetCurrentAccountId(IPrincipal user)
        {
            return new Guid(user.Identity.GetUserId()); // Return User ID using the UserIdClaim type
        }
        // Check if the profile belongs to the current user
        public bool EnsureIsUserProfile(Profile profile, IPrincipal user)
        {
            return profile.AccountId == new Guid(user.Identity.GetUserId());
        }

        /*
         * Image CRUD logic/operation - add save to database  
         */

        // Add new image to database
        public void Add(Image image)
        {
            unit.Images.Add(image);
        }

        /*
         * Subscription CRUD logic/operation - add save to database   
         */

        // Add new image to database
        public void Add(Subscription sub)
        {
            unit.Subscriptions.Add(sub);
        }

        /*
         * Forum (Posts & Comments) CRUD logic/operations
         */

        // Add new post to database
        public void Add(Post post)
        {
            unit.Posts.Add(post);
        }
        // Add new comment to database
        public void Add(Comment comment)
        {
            unit.Comments.Add(comment);
        }
        // Get all posts objects from database
        public IEnumerable<Post> GetPosts()
        {
            return unit.Posts.GetElements(x => x.PostId != 0);
        }
    }
}