namespace PortaCapena.OdooJsonRpcClient.Tests;

using FluentAssertions;
using Request;
using Xunit;

public class OdooRequestParamsTest
{
    private class TestShim(string url, string service, string method, params object[] parameters)
        : OdooRequestParams(url, service, method, parameters)
    {
        public static new object[] RemoveTrailingNulls(object[] input) => OdooRequestParams.RemoveTrailingNulls(input);
    }

    [Fact]
    public void Can_remove_trailing_nulls()
    {
        var input = new object[] { 1, "test", null, null };
        var result = TestShim.RemoveTrailingNulls(input);
        result.Length.Should().Be(2);
        result[0].Should().Be(1);
        result[1].Should().Be("test");
    }
    
    [Fact]
    public void Keeps_starting_nulls()
    {
        var input = new object[] { null, 1, "test" };
        var result = TestShim.RemoveTrailingNulls(input);
        result.Length.Should().Be(3);
        result[0].Should().Be(null);
        result[1].Should().Be(1);
        result[2].Should().Be("test");
    }

    [Fact]
    public void Can_handle_no_trailing_nulls()
    {
        var input = new object[] { 1, "test", "foo" };
        var result = TestShim.RemoveTrailingNulls(input);
        result.Length.Should().Be(3);
        result[0].Should().Be(1);
        result[1].Should().Be("test");
        result[2].Should().Be("foo");
    }
    
    [Fact]
    public void Can_handle_mixed_array()
    {
        var input = new object[] { 1, null, "test", null, null };
        var result = TestShim.RemoveTrailingNulls(input);
        result.Length.Should().Be(3);
        result[0].Should().Be(1);
        result[1].Should().Be(null);
        result[2].Should().Be("test");
    }

    [Fact]
    public void Can_handle_all_nulls()
    {
        var input = new object[] { null, null, null };
        var result = TestShim.RemoveTrailingNulls(input);
        result.Length.Should().Be(0);
    }
    
    [Fact]
    public void Can_handle_empty_array()
    {
        var input = new object[] {};
        var result = TestShim.RemoveTrailingNulls(input);
        result.Length.Should().Be(0);
    }
    
    [Fact]
    public void Can_handle_null_input()
    {
        object[] input = null;
        var result = TestShim.RemoveTrailingNulls(input);
        result.Length.Should().Be(0);
    }
}