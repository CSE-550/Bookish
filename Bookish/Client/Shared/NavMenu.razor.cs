using Blazored.LocalStorage;
using Bookish.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
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
        public bool IsLoggedIn { get; set; }
        
        [Inject]
        public NavigationManager NavigationManager { get; set; }

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
            IsLoggedIn = await (AuthProvider as AuthStateProvider).IsAuthorized();
            UserName = await LocalStorage.GetItemAsync<string>("user");
            NavigationManager.LocationChanged += LocationChanged;
            base.OnInitialized();

        }

        protected override async Task OnParametersSetAsync()
        {
            await SetUnreadMessages();
            await base.OnParametersSetAsync();
        }

        public async Task Logout()
        {
            AuthStateProvider authProvider = AuthProvider as AuthStateProvider;
            await authProvider.NotifyUserLogout();
        }
        async void LocationChanged(object sender, LocationChangedEventArgs e)
        {
            await SetUnreadMessages();
            StateHasChanged();
        }

        async Task SetUnreadMessages()
        {
            var res = await HttpClient.GetAsync("/api/amountmessages");
            Console.WriteLine(res.StatusCode);
            if (IsLoggedIn && res.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                IsLoggedIn = false;
                await Logout();
            }
            else if(res.IsSuccessStatusCode)
            {
                IsLoggedIn = true;
                var json = await res.Content.ReadAsStringAsync();
                UnreadMessages = int.Parse(json);
            }
        }
    }
}
