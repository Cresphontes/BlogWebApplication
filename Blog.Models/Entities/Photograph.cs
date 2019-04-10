using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Blog.Models.Abstracts;

namespace Blog.Models.Entities
{
    public class Photograph:BaseEntity<string>
    {
        public Photograph()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [DisplayName("Yol")]
        public string Path { get; set; }

        public string ArticleId { get; set; }

        [ForeignKey("ArticleId")]
        public virtual Article Article { get; set; }


    }
}
