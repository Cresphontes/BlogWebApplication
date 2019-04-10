
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Web;

namespace Blog.Models.ViewModels
{
   public class ArticleViewModel
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(100,ErrorMessage = "Başlığınız en fazla 100 karakter olabilir")]
        public string Title { get; set; }
       
        
        public DateTime ExistingTime { get; set; }

        [Required]
        public string Content { get; set; }
      
        public List<string> PhotoPath { get; set; }

        [Required]
        public string Category { get; set; }

        public int CommentsCount{ get; set; }

     
        public List<IFormFile> PostedFile { get; set; }
    }
}
