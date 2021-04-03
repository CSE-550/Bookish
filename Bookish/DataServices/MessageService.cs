using Bookish.Data;
using Bookish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<AuthMessageModel> GetMessages(AuthUserModel authUser, int page)
        {
            int? authUserId = authUser?.Id;
            List<AuthMessageModel> messages = context.Messages
                .Where(m => m.ForUser_Id == authUser.Id)
                .Skip((page - 1) * 25)
                .Take(25)
                .Select(m => new AuthMessageModel
                {
                    Id = m.Id,
                    Title = m.Title,
                    Seen = m.Seen,
                    Post = new PostListModel
                    {
                        Title = m.AboutComment.Commented_On.Title        
                    },
                    Comment = new CommentModel
                    {
                        Body = m.AboutComment.Body,
                        Commented_At = m.AboutComment.Commented_At,
                        Commented_By = m.AboutComment.Commented_By.Username,
                        Id = m.AboutComment.Id,
                        Rating = authUserId == null ? null : m.AboutComment.Ratings.Where(r => r.User_Id == authUserId).Select(r => new RatingModel { 
                            Id = r.Id,
                            Post_Id = r.Post_Id,
                            isUpvote = r.IsUpvoted
                        }).FirstOrDefault(),
                        PostTitle = m.AboutComment.Commented_On.Title,
                        Post_Id = m.AboutComment.Commented_On.Id,
                    }
                })
                .ToList();

            List<int> unreadIds = messages
                .Where(me => !me.Seen)
                .Select(me => me.Id)
                .ToList();

            if (unreadIds.Count() > 0)
            {
                List<Message> messagesDb = context.Messages
                    .Where(m => unreadIds.Contains(m.Id))
                    .ToList();

                messagesDb.ForEach(m => m.Seen = true);
                context.SaveChanges();
            }

            return messages;
        }

        public int UnreadMessages(AuthUserModel authUser)
        {
            return context.Messages
                .Where(m => m.ForUser_Id == authUser.Id && !m.Seen)
                .Count();
        }
    }
}
