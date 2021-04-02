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
            return context.Messages
                .Where(m => m.ForUser_Id == authUser.Id)
                .Skip((page - 1) * 25)
                .Take(25)
                .Select(m => new AuthMessageModel
                {
                    Title = m.Title,
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
        }
    }
}
