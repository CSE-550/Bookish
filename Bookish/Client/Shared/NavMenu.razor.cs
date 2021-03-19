using Blazored.LocalStorage;
using Bookish.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookish.Client.Shared
{
    public partial class NavMenu : ComponentBase
    {
        [Inject]
        public ILocalStorageService LocalStorage { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthProvider { get; set; }

        public string UserName { get; set; }

        private bool IsActive = false;

        protected override async Task OnInitializedAsync()
        {
            UserName = await LocalStorage.GetItemAsync<string>("user");
            base.OnInitialized();
        }

        public async Task Logout()
        {
            AuthStateProvider authProvider = AuthProvider as AuthStateProvider;
            authProvider.NotifyUserLogout();
        }
    }
}
