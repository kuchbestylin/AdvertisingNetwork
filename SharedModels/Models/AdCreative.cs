using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using SharedModels.Models;

namespace SharedModels.Models
{
    public class AdCreative
    {
        public int Id { get; set; }
        public int DisplayImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public string? Heading { get; set; }
        public double Price { get; set; }
        public DateTime? OfferEnding { get; set; }
        public string? HtmlString { get; set; }
        public double Discount { get; set; }
        public string? Description { get; set; }
        public string? AdvertLinkAddress { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
