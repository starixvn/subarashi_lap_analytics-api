using System;

namespace Fanex.Athena.AnalyticsAPI.Common.Models.Configuration
{
    [Serializable]
    public class Product
    {
        #region Field

        public const string SB = "SB";

        public const string SLT = "SLT";

        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public string ProductCode { get; set; }

        public bool AllowCommission { get; set; }

        public bool AllowWinloss { get; set; }

        public bool IsOfferedSingleWallet { get; set; }

        #endregion Field
    }
}