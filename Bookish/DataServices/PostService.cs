using Bookish.Data;
using Bookish.Models;
using System;
using System.Collections.Generic;

namespace Bookish.DataServices
{
    public class PostService : IPostService
    {
        private Context context { get; set; }

        public PostService(Context context)
        {
            this.context = context;
        }

        public List<PostListModel> GetPosts(int page, int countPerPage, string orderBy)
        {
            throw new NotImplementedException();
        }

        public PostModel GetPost(int id)
        {
            throw new NotImplementedException();
        }

        public PostModel CreatePost(PostModel postModel)
        {
            throw new NotImplementedException();
        }
    }
}
