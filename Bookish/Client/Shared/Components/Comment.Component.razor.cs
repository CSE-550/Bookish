using Bookish.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Bookish.Client.Shared.Components
{
    public class Comment : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        [Parameter]
        public CommentModel CommentModel { get; set; }

        [Parameter]
        public bool DisplayPostTitle { get; set; }

        [Parameter]
        public bool IsSub { get; set; }

        [Parameter]
        public string PostTitleOverride { get; set; }

        protected bool IsReplying { get; set; }

        protected bool IsCollapsed { get; set; }

        protected string CommentBody { get; set; }

        protected bool IsHidingChildren { get; set; }

        protected async void AddComment()
        {
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync("/api/comment", new CommentModel { 
               Body = CommentBody,
               Post_Id = CommentModel.Post_Id,
               Parent_Id = CommentModel.Id
            });

            if (CommentModel.Comments == null) CommentModel.Comments = new List<CommentModel>();
            CommentModel.Comments.Add(await response.Content.ReadFromJsonAsync<CommentModel>());

            CommentBody = null;
            IsReplying = false;

            StateHasChanged();
        }

        protected async void LoadRemaining()
        {
            List<CommentModel> comments = await HttpClient.GetFromJsonAsync<List<CommentModel>>($"/api/comment?commentId={CommentModel.Id}&skip={CommentModel.Comments.Count()}&take={CommentModel.TotalComments}");
            CommentModel.Comments.AddRange(comments);
            StateHasChanged();
        }

        protected string GetTimeSincePost()
        {
            TimeSpan timeSince = DateTime.Now - CommentModel.Commented_At;
            if (timeSince.TotalHours < 1)
            {
                return string.Format("{0:%m} min ago", timeSince);
            } 
            else if (timeSince.TotalHours < 24)
            {
                return string.Format("{0:%h} hours ago", timeSince);
            }
            else
            {
                return string.Format("{0:%d} days ago", timeSince);
            }
        }

        protected void Vote(CommentModel comment, int vote)
        {
            StateHasChanged();
        }

    }
}
