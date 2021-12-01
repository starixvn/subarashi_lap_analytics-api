using System;
using System.Collections.Generic;
using System.Text;

namespace Fanex.Athena.AnalyticsAPI.Common.Models.Configuration
{
    public class Licensee
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsAllowedCustomerComm { get; set; }

        public bool IsEnableBA { get; set; }
    }
}