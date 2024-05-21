using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SharedModels.Models
{
    public class RegisteredWebsite
    {
        public int Id { get; set; }
        public required string Domain { get; set; }
        public List<string>? ExcludedPages { get; set; }
        public List<string>? Categories { get; set; }
        public string? Continent { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? TagHashCode { get; set; }
        public string? BaseUri { get; set; }
        public PaymentType? MonetizationType { get; set; }
        public List<string>? Audience { get; set; }
        public bool AdsEnabled { get; set; }
        public bool HasScriptTag { get; set; }
        [ForeignKey(nameof(User))]
        public int UserID { get; set; }
        public User? User { get; set; }
    }
}
