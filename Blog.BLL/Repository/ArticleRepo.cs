using System;
using System.Collections.Generic;
using System.Text;
using Blog.DAL.Data;
using Blog.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.BLL.Repository
{
    public class ArticleRepo:RepositoryBase<Article,string>
    {
        private readonly ApplicationDbContext _dbContext;
        public ArticleRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
