using Blog.Models.Enums;
using Blog.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.BLL
{
    public interface IMessageService
    {
        MessageStates MessageStates { get; }

        Task SendAsync(MessageViewModel message, params string[] contacts);


    }
}
