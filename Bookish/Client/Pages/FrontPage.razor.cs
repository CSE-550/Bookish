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

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected List<PostListModel> Posts { get; set; }

        protected int Page { get; set; }

        protected int CountPerPage { get; set; }

        protected bool IsLoading { get; set; }

        protected bool IsEmpty { get; set; }

        protected override void OnInitialized()
        {
            Posts = new List<PostListModel>();
            Page = 1;
            CountPerPage = 25;
            IsEmpty = false;
            LoadPosts();
        }

        protected async void LoadPosts()
        {
            IsLoading = true;
            StateHasChanged();
            List<PostListModel> posts = await HttpClient.GetFromJsonAsync<List<PostListModel>>($"/api/postlist?page={Page}&countPerPage={CountPerPage}&orderBy=");
            if (posts == null || posts.Count() == 0)
            {
                IsEmpty = true;
            } 
            else
            {
                Posts.AddRange(posts);
            }
            IsLoading = false;
            StateHasChanged();
        }

        protected void OpenPost(PostListModel postListModel)
        {
            NavigationManager.NavigateTo($"/Post/View/{postListModel.Id}");
        }

        protected void LoadNextPage()
        {
            Page++;
            LoadPosts();
        }

    }
}
