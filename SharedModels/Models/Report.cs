using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SharedModels.Models
{
    public class Report
    {
        public int Id { get; set; }
        public List<Impression>? Impressions { get; set; }
        public int Clicks { get; set; }
        public List<Conversion> Conversions { get; set; }
        [ForeignKey(nameof(Campaign))]
        public int CampaignId { get; set; }
        public Campaign? Campaign { get; set; }

    }

    public record Impression(int Id, int SessionId, string Referrer, string PageUrl, string UserAgent, string IpAddress, string country, int duration, ConversionType ConversionType);
    public record Conversion(int Id, int SessionId, string Referrer, string PageUrl, string UserAgent, string IpAddress, string country, int duration, ConversionType ConversionType);
    public enum ConversionType
    {
        PURCHASE,
        SUBSCRIBE,
        VISIT
    }
}
