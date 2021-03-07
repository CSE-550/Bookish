using Bookish.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Bookish.Client.Pages.Post
{
    /// <summary>
    /// Allows a user to create a post
    /// </summary>
    public partial class CreatePost : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// The post the user is creating
        /// </summary>
        protected PostModel Post { get; set; }

        protected override void OnInitialized()
        {
            // Default to empty model
            Post = new PostModel();
        }

        /// <summary>
        /// Validate the post model, create the post, and redirect
        /// to view the post page
        /// </summary>
        protected async void HandleValidSubmit()
        {
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync("/api/post", Post);

            Post = await response.Content.ReadFromJsonAsync<PostModel>();

            // Redirect to the view post page
            NavigationManager.NavigateTo($"/Post/View/{Post.Id}");
        }

    }
}
