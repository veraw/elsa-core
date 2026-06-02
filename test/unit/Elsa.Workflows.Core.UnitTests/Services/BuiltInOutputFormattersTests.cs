using System.Data;
using Newtonsoft.Json.Linq;
using Elsa.Workflows.Models;

namespace Elsa.Workflows.Core.UnitTests.Services;

public class BuiltInOutputFormattersTests
{
    [Fact]
    public async Task DataSet_Formatter_Converts_Rows_To_Dictionary_Array()
    {
        var transformation = new DataSetToDictionaryArrayOutputTransformation();
        var dataSet = new DataSet();
        var table = new DataTable("Users");
        table.Columns.Add("Id", typeof(int));
        table.Columns.Add("Name", typeof(string));
        table.Rows.Add(1, "Alice");
        table.Rows.Add(2, "Bob");
        dataSet.Tables.Add(table);
        var context = new ActivityOutputTransformationContext(null!, null!, dataSet, transformation.GetDescriptor());

        var result = await transformation.TransformAsync(context);

        var rows = Assert.IsType<Dictionary<string, object?>[]>(result);
        Assert.Equal(2, rows.Length);
        Assert.Equal(1, rows[0]["Id"]);
        Assert.Equal("Alice", rows[0]["Name"]);
        Assert.Equal(2, rows[1]["Id"]);
        Assert.Equal("Bob", rows[1]["Name"]);
    }

    [Fact]
    public async Task JObject_Formatter_Converts_To_Dictionary_With_Nested_Types()
    {
        var transformation = new JObjectToDictionaryOutputTransformation();
        var jobject = JObject.Parse("""
            {
              "id": 1,
              "name": "alice",
              "active": true,
              "roles": ["admin", "author"],
              "profile": { "city": "Amsterdam" }
            }
            """);
        var context = new ActivityOutputTransformationContext(null!, null!, jobject, transformation.GetDescriptor());

        var result = await transformation.TransformAsync(context);

        var dictionary = Assert.IsType<Dictionary<string, object?>>(result);
        Assert.Equal(1L, dictionary["id"]);
        Assert.Equal("alice", dictionary["name"]);
        Assert.Equal(true, dictionary["active"]);
        Assert.IsType<List<object?>>(dictionary["roles"]);
        Assert.IsType<Dictionary<string, object?>>(dictionary["profile"]);
    }

    [Fact]
    public async Task JArray_Formatter_Converts_To_Dictionary_Array_And_Wraps_Primitives()
    {
        var transformation = new JArrayToDictionaryArrayOutputTransformation();
        var jarray = JArray.Parse("""
            [
              { "id": 1, "name": "alice" },
              42,
              "plain-text"
            ]
            """);
        var context = new ActivityOutputTransformationContext(null!, null!, jarray, transformation.GetDescriptor());

        var result = await transformation.TransformAsync(context);

        var rows = Assert.IsType<Dictionary<string, object?>[]>(result);
        Assert.Equal(3, rows.Length);
        Assert.Equal(1L, rows[0]["id"]);
        Assert.Equal("alice", rows[0]["name"]);
        Assert.Equal(42L, rows[1]["Value"]);
        Assert.Equal("plain-text", rows[2]["Value"]);
    }
}