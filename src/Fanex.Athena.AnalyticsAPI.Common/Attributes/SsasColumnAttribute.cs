namespace Fanex.Athena.AnalyticsAPI.Common.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class SsasColumnAttribute : Attribute
    {
        public SsasColumnAttribute(string mapColumn)
        {
            MapColumn = mapColumn;
        }

        public string MapColumn { get; set; }
    }
}