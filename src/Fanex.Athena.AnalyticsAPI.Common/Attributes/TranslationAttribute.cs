namespace Fanex.Athena.AnalyticsAPI.Common.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class TranslationAttribute : Attribute
    {
        public string PropertyId { get; private set; }

        public string TranslationType { get; private set; }

        public TranslationAttribute(string propertyId, string translationType = null)
        {
            PropertyId = propertyId;
            TranslationType = translationType;
        }
    }
}