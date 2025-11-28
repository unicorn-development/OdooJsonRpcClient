using System;
using System.ComponentModel;
using System.Reflection;
using PortaCapena.OdooJsonRpcClient.Consts;
using PortaCapena.OdooJsonRpcClient.Extensions;
using Xunit;

namespace PortaCapena.OdooJsonRpcClient.Tests;

public class OdooEnumExtensionTests
{
    public static string DescriptionOldReflexion(Enum value)
    {
        try
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return value.ToString();
        }
        catch (Exception)
        {
            return value.ToString();
        }
    }
    
    [Fact]
    public void TestOdooDescriptionExtension()
    {
        var enumValues = typeof(OdooOperator).GetEnumValues();
        foreach (OdooOperator enumValue in enumValues)
        {
            Assert.Equal(enumValue.Description(), DescriptionOldReflexion(enumValue));
        }
    }
}