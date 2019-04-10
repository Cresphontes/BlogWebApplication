using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Blog.Models.Abstracts
{
    public abstract class BaseEntity2<TId,TId2>:BaseEntity<TId>
    {
     
        [Key]
        [Column(Order = 2)]
        public TId2 Id2 { get; set; }

    }
}
