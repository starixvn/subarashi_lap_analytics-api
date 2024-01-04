using System.ComponentModel.DataAnnotations;

namespace Fanex.Athena.AnalyticsAPI.Models
{
    public class LapTokenRequest
    {
        private const int MaxLengthToken = 30;

        [Required]
        [StringLength(MaxLengthToken)]
        public string Token { get; set; }
    }
}