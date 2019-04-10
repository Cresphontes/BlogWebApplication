using System;
using System.Collections.Generic;
using System.Text;
using Blog.DAL.Data;
using Blog.Models.Entities;

namespace Blog.BLL.Repository
{
    public class CategoryRepo : RepositoryBase<Category, int>
    {
        private readonly ApplicationDbContext _dbContext;
        public CategoryRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

    
    }
}
