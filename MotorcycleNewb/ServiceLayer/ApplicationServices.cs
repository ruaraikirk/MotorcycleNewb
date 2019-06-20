using Microsoft.AspNet.Identity;
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
         * PROFILE CRUD operations 
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
        // Get profile object from a given ProfileId.
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
         * IMAGE CRUD operations 
         */
        // Add new image to database
        public void Add(Image image)
        {
            unit.Images.Add(image);
        }

        /*
         * SUBSCRIPTION CRUD operations 
         */
        // Add new image to database
        public void Add(Subscription sub)
        {
            unit.Subscriptions.Add(sub);
        }

        /*
         * FORUM (Posts & Comments) CRUD operations 
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

        /*
        // Retrieve a collection of only Post objects uploaded by current user and its Friends.
        public IEnumerable<Post> GetPostsFriends(int? userId)
        {
            //Get all friends by given user, then add to this collection the user itself.
            // Return. Takes all the posts and filter this collection by any profiles that belongs to friends collection
            var friends = GetFriends(userId).ToList();
            friends.Add(GetProfile(userId));
            return GetPosts().Where(y => friends.Any(x => x.ProfileId == y.ProfileId)).ToList();
        }
        */
        // Retrieve all posts written by the given user.
        public IEnumerable<Post> GetPostsByUser(int id)
        {
            IEnumerable<Post> posts = unit.Posts.GetElements(x => x.ProfileId == id);
            return posts;
        }
        /*
        /// <summary>
        /// Retrieve all the posts filter by given keywords (in an array of strings)
        /// </summary>
        /// <param name="hashtags">array of strings which represent the keywords to use for filtering</param>
        /// <param name="userID">Id of the current user</param>
        /// <returns>a collection of Post objects</returns>
        public List<Post> GetPostsByHashtags(string[] hashtags, int? userID)
        {
            //If hashtags array contains only one item then the returned collection will be posts where the single keyword is found.
            // If more than one keyword in array, get all posts that belong to all keywords and avoid duplicated posts
            // If array is empty get all the posts without filtering by any keyword
            var posts = new List<Post>();

            if (unit.MetaData != null)
            {
                if (hashtags.Length > 1)
                {

                    foreach (var item in hashtags)
                    {
                        posts.AddRange(GetPostsByHashtags(item, userID).Where(x => posts.All(y => y.Id != x.Id)));
                    }

                }
                else if (hashtags.Length == 1)
                {
                    posts = GetPostsByHashtags(hashtags[0], userID).ToList();
                }
                else
                {
                    posts = GetPostsFriends(userID).ToList();
                }
            }
            return posts;
        }*/
    }
}