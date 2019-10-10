using System;
using System.Collections.Generic;
using System.Text;

namespace Fanex.Athena.AnalyticsAPI.Core.Models.DTO
{
    public class AuthInfo
    {
        public string SiteName { get; set; }
        public string ActionLink { get; set; }
        public int UserSEQ { get; set; }
        public string UserName { get; set; }
    }
}
