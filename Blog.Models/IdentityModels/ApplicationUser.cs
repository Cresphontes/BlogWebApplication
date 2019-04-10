using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Blog.Models.IdentityModels
{
    public class ApplicationUser : IdentityUser
    {

        [Required]
        [MaxLength(30, ErrorMessage = "İsiminiz 30 karakterden fazla olamaz.")]
        [MinLength(2, ErrorMessage = "İsiminiz 2 karakterden az olamaz.")]
        public string Name { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage = "Soyisiminiz 30 karakterden fazla olamaz.")]
        [MinLength(2, ErrorMessage = "Soyisiminiz 2 karakterden az olamaz.")]
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
      


    }
}
