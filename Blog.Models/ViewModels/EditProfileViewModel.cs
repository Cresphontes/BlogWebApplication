using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Blog.Models.ViewModels
{
    public class EditProfileViewModel
    {
        [Required]
        [MaxLength(30,ErrorMessage = "İsiminiz 30 karakterden fazla olamaz.")]
        [MinLength(2, ErrorMessage = "İsiminiz 2 karakterden az olamaz.")]
        public string Name { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage = "Soyisiminiz 30 karakterden fazla olamaz.")]
        [MinLength(2, ErrorMessage = "Soyisiminiz 2 karakterden az olamaz.")]
        public string Surname { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [MaxLength(11, ErrorMessage = "Geçerli bir numara giriniz.")]
        [MinLength(11, ErrorMessage = "Geçerli bir numara giriniz.")]
        public string PhoneNumber { get; set; }
    }
}
