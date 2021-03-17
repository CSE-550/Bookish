using Blazored.LocalStorage;
using Bookish.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bookish.Client.Services
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient HttpClient;
        private readonly ILocalStorageService LocalStorage;

        public AuthStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
        {
            HttpClient = httpClient;
            LocalStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string token = await LocalStorage.GetItemAsync<string>("authToken");
            if (string.IsNullOrWhiteSpace(token))
            {
                // Not authenticated
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            // Add the bearer token to the auth header
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwtAuthType")));
        }

        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            List<Claim> claims = new List<Claim>();
            string payload = jwt.Split('.')[1];

            byte[] jsonBytes = ParseBase64WithoutPadding(payload);

            Dictionary<string, object> keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
            return claims;
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        public async Task NotifyUserLogin(AuthUserModel authUser)
        {
            await LocalStorage.SetItemAsync<string>("authToken", authUser.Token);
            await LocalStorage.SetItemAsync<string>("user", authUser.Username);

            AuthenticationState authState = await GetAuthenticationStateAsync();

            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public void NotifyUserLogout()
        {
            Task<AuthenticationState> authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
