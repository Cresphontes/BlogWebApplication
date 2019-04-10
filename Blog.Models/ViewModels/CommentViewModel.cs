using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Blog.Models.ViewModels
{
    public class CommentViewModel
    {

        public string Id { get; set; }
        public string ArticleId { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "İsiminiz 30 karakterden fazla olamaz.")]
        [MinLength(2, ErrorMessage = "İsiminiz 2 karakterden az olamaz.")]
        public string Name { get; set; }
        [Required]
        [DataType("Email")]
        public string Email { get; set; }
        [Required]
        [MaxLength(500, ErrorMessage = "Yorumunuz 500 karakterden fazla olamaz.")]
        public string Content { get; set; }

        public bool isConfirmed { get; set; }

        public string ExistingTime { get; set; }
    }
}
