using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SharedModels.Models
{
    public class Report
    {
        public int Id { get; set; }
        public List<Click>? Clicks { get; set; }
        public List<View>? Wiews { get; set; }
        public List<Hover>? Hovers { get; set; }
        public List<Conversion>? Conversions { get; set; }
        [ForeignKey(nameof(Campaign))]
        public int CampaignId { get; set; }
        public Campaign? Campaign { get; set; }

    }

    public class Click
    {
        public int Id { get; set; }
        public DateTime? Created { get; set; }
    }

    public class View
    {
        public int Id { get; set; }
        public DateTime? Created { get; set; }
    }
    public class Hover
    {
        public int Id { get; set; }
        public DateTime? Created { get; set; }
        public double Duration { get; set; }
    }

    public class Impression
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public string Referrer { get; set; }
        public string PageUrl { get; set; }
        public string UserAgent { get; set; }
        public string IpAddress { get; set; }
        public string Country { get; set; }
        public DateTime? Created { get; set; }
        public int Duration { get; set; }
        [ForeignKey(nameof(RegisteredWebsite))]
        public int RegisteredWebsiteID {  get; set; }
        public RegisteredWebsite RegisteredWebsite { get; set; }
    }

    public class Conversion
    {
        public int Id { get; set; }
        public DateTime? Created { get; set; }
        public string? Referrer { get; set; }
        public string? PageUrl { get; set; }
        public string? UserAgent { get; set; }
        public string? IpAddress { get; set; }
        public string? country { get; set; }
    }
}
