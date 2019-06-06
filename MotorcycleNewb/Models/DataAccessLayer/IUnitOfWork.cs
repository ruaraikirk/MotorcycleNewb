﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleNewb.Models.DataAccessLayer
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Profile> Profiles { get; set; }
        IRepository<Image> Images { get; set; }
        //ToDo
        IRepository<Comment> Comments { get; set; }
        IRepository<Post> Posts { get; set; }

        void Update(Profile element);
        void Delete(Profile element);
        void Commit();
    }
}
