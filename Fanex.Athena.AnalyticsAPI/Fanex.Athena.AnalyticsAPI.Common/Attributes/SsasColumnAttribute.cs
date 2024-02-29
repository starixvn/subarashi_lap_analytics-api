using System;

namespace Fanex.Athena.AnalyticsAPI.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class SsasColumnAttribute : Attribute
    {
        public SsasColumnAttribute(string mapColumn)
        {
            MapColumn = mapColumn;
        }

        public string MapColumn { get; set; }
    }
}