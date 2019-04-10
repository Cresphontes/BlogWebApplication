using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Blog.Models.Abstracts;
using Blog.Models.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Blog.Models.Entities
{
    public class Category:BaseEntity<int>
    {
       public string Name { get; set; }
  
       public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
    }
                
}
