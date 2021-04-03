using Blazored.LocalStorage;
using Bookish.Client.Services;
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
    public partial class History : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        public ILocalStorageService LocalStorage { get; set; }

        protected List<IListItem> HistoryItems { get; set; }

        protected int Page { get; set; }

        protected bool IsLoading { get; set; }

        protected bool IsEmpty { get; set; }

        protected string UserName { get; set; }

        protected override async Task OnInitializedAsync()
        {
            HistoryItems = new List<IListItem>();
            UserName = await LocalStorage.GetItemAsync<string>("user");
            LoadMoreItems();
        }

        protected async void LoadItems()
        {
            IsLoading = true;
            StateHasChanged();
            HistoryModel historyModel = await HttpClient.GetFromJsonAsync<HistoryModel>($"/api/history?page={Page}");
            List<IListItem> items = historyModel.GetItems();
            IsEmpty = items.Count() == 0;
            HistoryItems.AddRange(items);
            StateHasChanged();
        }

        protected void LoadMoreItems()
        {
            Page++;
            LoadItems();
        }
    }
}



