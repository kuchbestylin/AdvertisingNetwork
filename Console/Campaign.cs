using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SharedModels.Models
{
    public class Campaign
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? AdvertStyle { get; set; }
        public string? Tags { get; set; }
        public List<string>? SafetyComponents { get; set; }
        public PaymentType? PaymentType { get; set; } = Models.PaymentType.CPT;
        public List<string>? TargetSites { get; set; }
        public int DailyBudget { get; set; }
        public bool DailyBudgetSet { get; set; }
        public int MaximumDailyImpressions { get; set; }
        public bool MaximumDailyImpressionsSet { get; set; }
        public int MaxBiddingAmount { get; set; }
        public bool MaxBiddingAmountSet { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey(nameof(AdCreative))]
        public int AdCreativeId { get; set; }
        public AdCreative? AdCreative { get; set; }
        public string? Orientation { get; set; }
        public DateTime? Created { get; set; }
        public int CampaignLengthInDays { get; set; }
        public bool ConversionsEnabled { get; set; }
        public string? AdvertLinkAddress { get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User? User { get; set; }
        
    }
    public class User
    {
        public int Id { get; set; }
        public string? Sub { get; set; }
        public string? NameIdentifier { get; set; }
        public string? Email { get; set; }
    }
    public enum PaymentType
    {
        CPC,
        CPT
    }

    public enum AdvertOrientation
    {
        BOX,
        HORIZONTAL,
        VERTICAL
    }
}
