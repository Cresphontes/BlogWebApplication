using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Blog.Models.ViewModels
{
    public class EditArticleViewModel
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Başlığınız en fazla 100 karakter olabilir")]

        public string Title { get; set; }
        [Required]
        public DateTime ExistingTime { get; set; }
   
        [Required]
        public string Category { get; set; }
    }
}
