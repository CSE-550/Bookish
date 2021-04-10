using Bookish.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Bookish.Client.Pages
{

    public partial class FrontPage : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        protected List<PostListModel> Posts { get; set; }

        protected int Page { get; set; }

        protected int CountPerPage { get; set; }

        protected bool IsLoading { get; set; }

        protected bool IsEmpty { get; set; }

        protected bool IsAllPosts { get; set; }

        protected string OrderPostsBy { get; set; }

       protected override async Task OnInitializedAsync()
        {
            Posts = new List<PostListModel>();
            Page = 1;
            CountPerPage = 25;
            await LoadPosts();
        }

        protected async Task LoadPosts()
        {
            IsLoading = true;
            List<PostListModel> posts = await HttpClient.GetFromJsonAsync<List<PostListModel>>($"/api/postlist?page={Page}&countPerPage={CountPerPage}&orderBy={OrderPostsBy}");
            if (posts.Count() < CountPerPage)
            {
                IsAllPosts = true;
            }

            Posts.AddRange(posts);
            IsLoading = false;
            StateHasChanged();
        }

        
        protected async Task LoadNextPage()
        {
            if (IsLoading || IsAllPosts) return;
            Page++;
            await LoadPosts();
        }

        protected async Task SetOrderPostsBy(ChangeEventArgs e)
        {
            OrderPostsBy = e.Value.ToString();
            await LoadPosts();
        }

    }
}
