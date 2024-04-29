using Microsoft.AspNetCore.SignalR;
using SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;

namespace SharedModels.Services
{
    public class DataStore : IDataStore
    {
        private readonly HttpClient _httpClient;
        const string sellsideconstant = "/sellside";
        const string demandsideconstant = "/demandside";

        public DataStore(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Campaign?> GetCampaignAsync(string campaignId)
        {
            return await _httpClient.GetFromJsonAsync<Campaign>($"{sellsideconstant}/adcampaign/{campaignId}");
        }

        public async Task<List<Campaign>?> GetCampaignsAsync(int userId)
        {
            return await _httpClient.GetFromJsonAsync<List<Campaign>>($"{sellsideconstant}/adcampaigns/{userId}");
        }

        public async Task<List<RegisteredWebsite>?> GetSitesAsync(int userId)
        {
            return await _httpClient.GetFromJsonAsync<List<RegisteredWebsite>>($"{demandsideconstant}/sites/{userId}");
        }

        public async Task<User?> GetUserAsync(User user)
        {
            var result = await _httpClient.PostAsJsonAsync<User>("/users", user);
            return await result.Content.ReadFromJsonAsync<User>();
        }

        public async Task<Campaign?> PostCampaignAsync(Campaign campaign)
        {
            var result =  await _httpClient.PostAsJsonAsync<Campaign>($"{sellsideconstant}/adcampaign", campaign);
            return await result.Content.ReadFromJsonAsync<Campaign>();
        }
        public async Task<Campaign?> PostCampaignAsync(Campaign campaign, string imagePath)
        {
            byte[] imageData = await File.ReadAllBytesAsync(imagePath);

            var formData = new MultipartFormDataContent();

            var campaignContent = new StringContent(JsonSerializer.Serialize(campaign), Encoding.UTF8, "application/json");
            formData.Add(campaignContent, "campaign");

            var imageContent = new ByteArrayContent(imageData);
            imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg"); // Assuming JPEG format, change as needed
            formData.Add(imageContent, "image", Path.GetFileName(imagePath));
            var result =  await _httpClient.PostAsync($"/file", formData);
            var responseContent = await result.Content.ReadAsStringAsync();
            Console.WriteLine(responseContent);
            return await result.Content.ReadFromJsonAsync<Campaign>();
        }

        public async Task<RegisteredWebsite?> PostSiteAsync(RegisteredWebsite site)
        {
            var result = await _httpClient.PostAsJsonAsync<RegisteredWebsite>($"{demandsideconstant}/sites", site);
            return await result.Content.ReadFromJsonAsync<RegisteredWebsite>();
        }

        public async Task<int> PostUserAsync<User>(User user)
        {
            var result = await _httpClient.PostAsJsonAsync<User>("/users", user);
            if (result.IsSuccessStatusCode)
                return 1;

            return 0;
        }
    }
}
