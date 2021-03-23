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
    public class PostList : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public PostListModel Post { get; set; }

        [Parameter]
        public int Page { get; set; }

        protected bool IsLoading { get; set; }

        protected bool IsEmpty { get; set; }

        protected void OpenPost(PostListModel postListModel)
        {
            NavigationManager.NavigateTo($"/Post/View/{postListModel.Id}");
        }

        protected void Vote(int vote)
        {
            Post.Votes += vote;
        }
    }
}
