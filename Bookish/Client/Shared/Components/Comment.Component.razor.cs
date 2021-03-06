﻿using Bookish.Models;
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

        protected bool IsReplying { get; set; }

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

            StateHasChanged();
        }

        protected async void LoadRemaining()
        {
            List<CommentModel> comments = await HttpClient.GetFromJsonAsync<List<CommentModel>>($"/api/comment?commentId={CommentModel.Id}&skip={CommentModel.Comments.Count()}&take={CommentModel.TotalComments}");
            CommentModel.Comments.AddRange(comments);
            StateHasChanged();
        }

    }
}
