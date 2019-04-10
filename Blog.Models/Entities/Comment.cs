using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Blog.Models.Abstracts;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Blog.Models.Entities
{
    public class Comment:BaseEntity<string>
    {
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

        [Required]
        public DateTime ExistingTime { get; set; }

        public bool isConfirmed { get; set; }

        public string ArticleId { get; set; }
        [ForeignKey("ArticleId")]
        public virtual Article Article { get; set; }
    }
}
