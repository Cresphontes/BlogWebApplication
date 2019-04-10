using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Blog.Models.Abstracts
{
    public abstract class BaseEntity<TId>
    {
        [Key]
        [Column(Order = 1)]
        public TId Id { get; set; }

    }
}
