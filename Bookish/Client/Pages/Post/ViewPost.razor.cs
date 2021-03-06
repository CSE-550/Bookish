﻿using Bookish.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Bookish.Client.Pages.Post
{
    public partial class ViewPost : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        protected PostModel Model { get; set; }

        protected string CommentBody { get; set; }

        [Parameter]
        public int Id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Model = await Http.GetFromJsonAsync<PostModel>("/api/post?id=" + Id);
        }

        public async void CreateComment()
        {
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync("/api/comment", new CommentModel { 
               Body = CommentBody,
               Post_Id = Model.Id
            });

            Model.Comments.Add(await response.Content.ReadFromJsonAsync<CommentModel>());

            StateHasChanged();
        }
    }
}
