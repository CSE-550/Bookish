using Blazored.LocalStorage;
using Bookish.Client.Services;
using Bookish.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Bookish.Client.Pages
{
    public partial class Login : ComponentBase
    {

        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        public ILocalStorageService LocalStorage { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthProvider { get; set; }

        protected UserLoginModel UserLoginModel { get; set; }

        protected bool DisplayError { get; set; }

        protected override void OnInitialized()
        {
            UserLoginModel = new UserLoginModel();
            base.OnInitialized();
        }

        public async Task HandleValidSubmit()
        {
            HttpResponseMessage loginResponse = await HttpClient.PostAsJsonAsync("/api/login", UserLoginModel); 

            AuthUserModel authUser = await loginResponse.Content.ReadFromJsonAsync<AuthUserModel>();

            if (authUser == null || authUser.Token == null || authUser.Token == "")
            {
                DisplayError = true;
            }
            else
            {
                DisplayError = false;
                AuthStateProvider authProvider = AuthProvider as AuthStateProvider;
                await authProvider.NotifyUserLogin(authUser);
                NavigationManager.NavigateTo("/");
            }
        }

    }
}
