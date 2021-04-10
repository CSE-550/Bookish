using Bookish.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
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
        public IJSRuntime jsRuntime { get; set; }

        protected List<PostListModel> Posts { get; set; }

        protected int Page { get; set; }

        protected int CountPerPage { get; set; }

        protected bool IsLoading { get; set; }

        protected bool IsEmpty { get; set; }

        protected bool IsAllPosts { get; set; }

        protected string OrderPostsBy { get; set; }

        private DotNetObjectReference<FrontPage> objectRef;

        protected override async Task OnInitializedAsync()
        {
            Posts = new List<PostListModel>();
            Page = 1;
            CountPerPage = 25;
            await LoadPosts();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                objectRef = DotNetObjectReference.Create(this);
                await jsRuntime.InvokeAsync<dynamic>("Observer.Initialize", objectRef, "PostLoadTriggerId");
            }
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
            Posts.Clear();
            IsAllPosts = false;
            Page = 1;
            await LoadPosts();
        }

        [JSInvokable]
        public async Task OnIntersection()
        {
            await LoadNextPage();
        }

    }
}
