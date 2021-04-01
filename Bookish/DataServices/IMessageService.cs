using Bookish.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookish.DataServices
{
    public interface IMessageService
    {
        void CreateMessage(AuthMessageModel messageModel, int forUserId);
    }
}
