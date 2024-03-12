using System;
using System.Collections.Generic;
using System.Text;

namespace SharedModels.Models
{
    public class TokenProvider
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
