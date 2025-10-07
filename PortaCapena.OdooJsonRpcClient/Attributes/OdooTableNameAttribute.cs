using System;

namespace PortaCapena.OdooJsonRpcClient.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class OdooTableNameAttribute : Attribute
    {
        public string Name;
        public double Version;

        public OdooTableNameAttribute(string name)
        {
            Name = name;
            Version = 1.0;
        }
    }
}