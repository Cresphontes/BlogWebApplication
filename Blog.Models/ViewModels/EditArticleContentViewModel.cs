using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Blog.Models.ViewModels
{
    public class EditArticleContentViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
        //[Required]
        //[DisplayName("Fotoğraf")]
        //public List<string> PhotoPath { get; set; }

        //public List<IFormFile> PostedFile { get; set; }
    }
}
