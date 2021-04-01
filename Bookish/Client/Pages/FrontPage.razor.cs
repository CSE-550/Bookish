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

        protected bool IsMinPosts { get; set; }

        protected string Sort { get; set; }

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
            Posts.Clear();
            StateHasChanged();
            List<PostListModel> posts = await HttpClient.GetFromJsonAsync<List<PostListModel>>($"/api/postlist?page={Page}&countPerPage={CountPerPage}&orderBy=");
            if (posts == null || posts.Count() == 0)
            {
                IsEmpty = true;
            }
            else
            {
                if (posts.Count() <= CountPerPage)
                {
                    IsMinPosts = true;
                }

                Posts.AddRange(posts);
            }
            IsLoading = false;
            StateHasChanged();
        }

        
        protected void LoadNextPage()
        {
          LoadPosts();
        }

        protected void setSort(ChangeEventArgs e)
        {
            Sort = e.Value.ToString();
            LoadPosts();
        }

    }
}
