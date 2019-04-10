using System;
using System.Collections.Generic;
using System.Text;
using Blog.DAL.Data;
using Blog.Models.Entities;

namespace Blog.BLL.Repository
{
    public class PhotographRepo:RepositoryBase<Photograph,string>
    {
     
        private readonly ApplicationDbContext _dbContext;
        public PhotographRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
