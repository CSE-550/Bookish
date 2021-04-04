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

        /// <summary>
        /// Gets the state of the current authorized user - if there is one - otherwise
        /// returns a default unauthorized user
        /// </summary>
        /// <returns>
        /// A task of generating the authentication state
        /// </returns>
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

        /// <summary>
        /// Parses the claims from the JWT
        /// </summary>
        /// <param name="jwt">The json web token to parse</param>
        /// <returns>
        /// A list of claims
        /// </returns>
        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            List<Claim> claims = new List<Claim>();
            string payload = jwt.Split('.')[1];

            byte[] jsonBytes = ParseBase64WithoutPadding(payload);

            Dictionary<string, object> keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
            foreach (var keyValue in keyValuePairs)
            {
                Console.WriteLine("{0}: {1}", keyValue.Key, keyValue.Value);
            }
            return claims;
        }

        /// <summary>
        /// Removes extra padding in the base64
        /// </summary>
        /// <param name="base64">The current base64 string</param>
        /// <returns>
        /// An unpadded base64 string
        /// </returns>
        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        /// <summary>
        /// Notifies the application of the login event
        /// swapping any views to the authorized view
        /// </summary>
        /// <param name="authUser">The current authorized user</param>
        /// <returns>
        /// A task of notifying
        /// </returns>
        public async Task NotifyUserLogin(AuthUserModel authUser)
        {
            await LocalStorage.SetItemAsync<string>("authToken", authUser.Token);
            await LocalStorage.SetItemAsync<string>("user", authUser.Username);

            AuthenticationState authState = await GetAuthenticationStateAsync();

            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        /// <summary>
        /// Notifies the application of a logout swapping the
        /// authorized view to the unauthorized view
        /// </summary>
        /// <returns>
        /// A task of notifying
        /// </returns>
        public async Task NotifyUserLogout()
        {
            await LocalStorage.RemoveItemAsync("authToken");
            Task<AuthenticationState> authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            NotifyAuthenticationStateChanged(authState);
        }
        public async Task<bool> IsAuthorized()
        {
            string token = await LocalStorage.GetItemAsync<string>("authToken");
            return !string.IsNullOrEmpty(token);
        }
    }
}
