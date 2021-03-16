using Bookish.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Bookish.Client.Pages
{
    public partial class SignUp : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public UserSignUpModel UserSignUpModel { get; set; }

        protected override void OnInitialized()
        {
            UserSignUpModel = new UserSignUpModel();
            base.OnInitialized();
        }

        public async void HandleValidSubmit()
        {
            // Submit signup
            HttpResponseMessage signUpResponse = await HttpClient.PutAsJsonAsync("/api/signup", UserSignUpModel);

            signUpResponse.EnsureSuccessStatusCode();

            // Account created navigate to login page
        }

    }
}
