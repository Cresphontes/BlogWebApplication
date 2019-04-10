using System;
using System.Collections.Generic;
using System.Text;
using Blog.DAL.Data;
using Blog.Models.Entities;

namespace Blog.BLL.Repository
{
    public class CommentRepo:RepositoryBase<Comment,string>
    {
        private readonly ApplicationDbContext _dbContext;
        public CommentRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
