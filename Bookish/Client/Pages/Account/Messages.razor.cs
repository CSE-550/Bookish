using Blazored.LocalStorage;
using Bookish.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Bookish.Client.Pages.Account
{
    public partial class Messages : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        public ILocalStorageService LocalStorage { get; set; }

        public List<AuthMessageModel> MessageModels { get; set; }

        public string UserName { get; set; }

        public int Page { get; set; }

        public bool IsLoading { get; set; }

        public bool IsEmpty { get; set; }
        
        protected override async Task OnInitializedAsync()
        {
            UserName = await LocalStorage.GetItemAsync<string>("user");
            MessageModels = new List<AuthMessageModel>();
            LoadMoreItems();
        }

        protected void LoadMoreItems()
        {
            Page++;
            LoadItems();
        }

        protected async void LoadItems()
        {
            IsLoading = true;
            StateHasChanged();
            List<AuthMessageModel> messages = await HttpClient.GetFromJsonAsync<List<AuthMessageModel>>($"/api/messages?page={Page}");
            IsEmpty = messages == null || messages.Count() == 0;
            if (messages != null)
            {
                MessageModels.AddRange(messages);
            }
            StateHasChanged();
        }
    }
}
