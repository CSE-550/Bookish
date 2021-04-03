using Blazored.LocalStorage;
using Bookish.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Bookish.Client.Shared
{
    public partial class NavMenu : ComponentBase
    {
        [Inject]
        public ILocalStorageService LocalStorage { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthProvider { get; set; }

        [Inject]
        public HttpClient HttpClient { get; set; }

        public string UserName { get; set; }

        private bool IsActive = false;

        public int UnreadMessages = 0;

        protected override async Task OnInitializedAsync()
        {
            UserName = await LocalStorage.GetItemAsync<string>("user");
            base.OnInitialized();
        }

        protected override async Task OnParametersSetAsync()
        {
            UnreadMessages = await HttpClient.GetFromJsonAsync<int>("/api/amountmessages");
            await base.OnParametersSetAsync();
        }

        public async Task Logout()
        {
            AuthStateProvider authProvider = AuthProvider as AuthStateProvider;
            await authProvider.NotifyUserLogout();
        }
    }
}
