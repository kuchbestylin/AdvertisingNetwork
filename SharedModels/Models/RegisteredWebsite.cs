using System;
using System.Collections.Generic;
using System.Text;

namespace SharedModels.Models
{
    public class RegisteredWebsite
    {
        public global::System.Int32 Id { get; set; }
        public string Address { get; set; }
        public List<string> ExcludedPages { get; set; }
        public bool IsActive { get; set; }
    }
}
