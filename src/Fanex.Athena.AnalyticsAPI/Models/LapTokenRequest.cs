namespace Fanex.Athena.AnalyticsAPI.Models
{
    using System.ComponentModel.DataAnnotations;

    public class LapTokenRequest
    {
        private const int MaxLengthToken = 30;

        [Required]
        [StringLength(MaxLengthToken)]
        public string Token { get; set; }
    }
}