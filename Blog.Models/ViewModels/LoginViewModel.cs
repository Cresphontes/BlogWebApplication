using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Blog.Models.ViewModels
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
       
        public string Password { get; set; }
       
        public bool RememberMe { get; set; }
    }
}
