using System;
using System.Collections.Generic;
using SharedModels.Models;
using System.Text;

namespace SharedModels.Services
{
    public interface IDataStore
    {
        //- Post User takes User from body and returns status
        //- Get /users/{string sub} take string sub from query and returns user/badrequest

        // /sellside
        //- Post /adcampaign Takes AdCampaign from body + file from http context. returns campaign/badrequest
        //- Get /adcampaign{string campaignId} takes campaignid as query. returns campaign/badrequest
        //- Get /adcampaigns{int uid} takes local userid as query. returns campaigns/badrequest

        // /demandside
        // Get /sites/{int uid} takes local userid as query. returns sites/badrequest
        // post /sites takes registerdsite from body. returns added site/badrequest

        // base url
        Task<User?> GetUserAsync(User user);
        Task<int> PostUserAsync<User>(User user);

        // sellside url (group)
        Task<Campaign?> PostCampaignAsync(Campaign campaign, string filePath);
        Task<Campaign?> GetCampaignAsync(string campaignId);
        Task<List<Campaign>?> GetCampaignsAsync(int userId);

        // demandside url (group)
        Task<List<RegisteredWebsite>?> GetSitesAsync(int userId);
        Task<RegisteredWebsite?> PostSiteAsync(RegisteredWebsite site);

    }
}
