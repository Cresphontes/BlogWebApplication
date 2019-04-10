using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Blog.Models.ViewModels
{
    public class EditPasswordViewModel
    {
        [Required(ErrorMessage = "Mevcut şifre alanı boş olamaz")]
        [DataType("Password")]
        public string CurrentPassword { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = " Yeni Şifreniz 6 karakterden az olamaz.")]
        [DataType("Password")]
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword",ErrorMessage = "Şifreler birbiri ile uyuşmuyor.")]
        [DataType("Password")]
        public string ConfirmNewPassword { get; set; }
      
    }
}
