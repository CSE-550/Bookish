using Bookish.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bookish.Client.Shared.Components
{
    public class VoteArrows : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        [Parameter]
        public int Votes { get; set; }

        [Parameter]
        public RatingModel Model { get; set; }

        [Parameter]
        public int PostId { get; set; }

        [Parameter]
        public int CommentId { get; set; }

        public async void Vote(int vote)
        {
            HttpResponseMessage response;
            if (Model == null)
            {
                // New vote
                response = await HttpClient.PutAsJsonAsync("/api/rating", new RatingModel { 
                    Comment_Id = CommentId,
                    Post_Id = PostId,
                    isUpvote = vote == 1
                });

            } 
            else
            {
                Model.isUpvote = vote == 1;
                // Updating a current vote
                response = await HttpClient.PatchAsync("/api/rating", new StringContent(JsonSerializer.Serialize(Model), Encoding.UTF8, "application/json-patch+json"));
            }

            Model = await response.Content.ReadFromJsonAsync<RatingModel>();
            if (Model.isUpvote) Votes++;
            else Votes--;
            StateHasChanged();
        }

    }
}
