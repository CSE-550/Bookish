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

        /// <summary>
        /// The user login model for logging in a specified user
        /// </summary>
        protected UserLoginModel UserLoginModel { get; set; }

        /// <summary>
        /// Display error on login
        /// </summary>
        protected bool DisplayError { get; set; }

        protected override void OnInitialized()
        {
            UserLoginModel = new UserLoginModel();
            base.OnInitialized();
        }

        /// <summary>
        /// On form valid submit. Attempt to login
        /// the user and notify the application of the 
        /// new login user
        /// </summary>
        /// <returns>
        /// Async Task of logging in a user
        /// </returns>
        public async Task HandleValidSubmit()
        {
            // Make request to login the user
            HttpResponseMessage loginResponse = await HttpClient.PostAsJsonAsync("/api/login", UserLoginModel); 

            // Get the auth user model
            AuthUserModel authUser = await loginResponse.Content.ReadFromJsonAsync<AuthUserModel>();

            // If unsuccesful display an error
            if (authUser == null || authUser.Token == null || authUser.Token == "")
            {
                DisplayError = true;
            }
            // Otherwise set the auth provider and notify the rest of the app
            else
            {
                DisplayError = false;
                AuthStateProvider authProvider = AuthProvider as AuthStateProvider;
                await authProvider.NotifyUserLogin(authUser);
                NavigationManager.NavigateTo("/", true);
            }
        }

    }
}
