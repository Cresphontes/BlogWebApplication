using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Models.ViewModels
{
    public class ResponseData
    {
        public string Message { get; set; }
        public object Data { get; set; }
        public bool Success { get; set; }
        public DateTime ResponseDate { get; set; }= DateTime.Now;
    }
}
