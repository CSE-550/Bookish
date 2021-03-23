using Bookish.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Bookish.Client.Pages.History
{
    public partial class History : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        protected List<PostListModel> Posts { get; set; }

        protected int Page { get; set; }

        protected int CountPerPage { get; set; }

        protected bool IsLoading { get; set; }

        protected override void OnInitialized()
        {
            Posts = new List<PostListModel>();
            Page = 1;
            CountPerPage = 25;
            LoadPosts();
        }

        protected async void LoadPosts()
        {
            IsLoading = true;
            StateHasChanged();
            List<PostListModel> posts = await HttpClient.GetFromJsonAsync<List<PostListModel>>($"/api/postlist/myactivity?page={Page}&countPerPage={CountPerPage}&orderBy=");
            Posts.AddRange(posts);
            StateHasChanged();
        }


        protected void LoadNextPage()
        {
            LoadPosts();
        }
    }
}



