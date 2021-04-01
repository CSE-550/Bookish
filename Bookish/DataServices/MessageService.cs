using Bookish.Data;
using Bookish.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookish.DataServices
{
    public class MessageService : IMessageService
    {
        private Context context { get; set; }

        public MessageService(Context context)
        {
            this.context = context;
        }

        public void CreateMessage(AuthMessageModel messageModel, int forUserId)
        {
            context.Messages.Add(new Message
            {
                Title = messageModel.Title,
                AboutComment_Id = messageModel.Comment.Id,
                ForUser_Id = forUserId
            });

            context.SaveChanges();
        }
    }
}
