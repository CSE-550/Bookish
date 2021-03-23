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
    public partial class PostList : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        
        public NavigationManager NavigationManager { get; set; }
       
        private List<PostListModel>  posts;
        [Parameter]
        public  List<PostListModel> Posts
        {
            get { return posts; }
            set { posts = value; LoadPosts(); }
        }

        [Parameter]
        public int Page { get; set; }

        protected bool IsLoading { get; set; }

        protected bool IsEmpty { get; set; }

        protected override void OnInitialized()
        {
            Posts = new List<PostListModel>();
            Page = 1;
            IsEmpty = false;
            LoadPosts();
        }

        protected void LoadPosts()
        {
            IsEmpty = false;
            if (Posts == null || Posts.Count() == 0)
            {
                IsEmpty = true;
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
         }

    }
}
