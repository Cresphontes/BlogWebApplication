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
    public class Article:BaseEntity<string>
    {

        public Article()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Required]
        [DisplayName("Başlık")]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [DisplayName("Oluşturulma Zamanı")]
        public DateTime ExistingTime { get; set; }
        [Required]
        [DisplayName("İçerik")]
        public string Content { get; set; }
        [Required]
        [DisplayName("Fotoğraf")]
        [NotMapped]
        public List<string> PhotoPath { get; set; }


        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public virtual ICollection<Photograph> Photographs { get; set; } = new List<Photograph>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    }
}
