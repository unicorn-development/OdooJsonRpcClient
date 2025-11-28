using PortaCapena.OdooJsonRpcClient.Converters;
using System;

namespace PortaCapena.OdooJsonRpcClient.Extensions
{
    public static partial class EnumExtension
    {
        public static string OdooValue(this Enum value)
        {
            var type = value.GetType().GetField(value.ToString());
            return OdooModelMapper.GetOdooEnumName(type);
        }
    }
}