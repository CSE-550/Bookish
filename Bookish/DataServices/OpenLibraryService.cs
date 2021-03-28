using Bookish.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bookish.DataServices
{
    public class OpenLibraryService
    {
        public HttpClient Client { get; }

        public OpenLibraryService(HttpClient client)
        {
            client.BaseAddress = new Uri("https://openlibrary.org/");
            Client = client;
        }

        public async Task<OpenLibraryBook> GetBookInformation(string isbn)
        {
            HttpResponseMessage response = await Client.GetAsync($"/isbn/{isbn}.json");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            using (var reader = new StreamReader(responseStream, Encoding.UTF8))
            {
                string value = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<OpenLibraryBook>(value, new JsonSerializerSettings());
            }
        }

        public async Task<OpenLibraryAuthor> GetAuthorInformation(string url)
        {
            HttpResponseMessage response = await Client.GetAsync($"{url}.json");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            using (var reader = new StreamReader(responseStream, Encoding.UTF8))
            {
                string value = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<OpenLibraryAuthor>(value, new JsonSerializerSettings());
            }
        }
    }
}
