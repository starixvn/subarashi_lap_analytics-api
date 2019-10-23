using System;

namespace Fanex.Athena.LGAPI.SSAS.Bridge.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class SsasColumnAttribute : Attribute
    {
        public SsasColumnAttribute(string mapColumn)
        {
            MapColumn = mapColumn;
        }

        public SsasColumnAttribute()
        {
        }

        public string MapColumn { get; set; }
    }
}