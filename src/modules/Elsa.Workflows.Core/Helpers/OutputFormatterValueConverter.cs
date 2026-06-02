using System.Data;
using Newtonsoft.Json.Linq;

namespace Elsa.Workflows.Helpers;

/// <summary>
/// Converts framework-specific output values into JavaScript-friendly CLR shapes.
/// </summary>
public static class OutputTransformationValueConverter
{
    /// <summary>
    /// Converts a <see cref="JToken"/> value into CLR primitives, dictionaries and lists.
    /// </summary>
    public static object? ConvertJToken(JToken? token)
    {
        if (token == null)
            return null;

        return token.Type switch
        {
            JTokenType.Object => ConvertJObject((JObject)token),
            JTokenType.Array => ((JArray)token).Select(ConvertJToken).ToList(),
            JTokenType.Integer => token.Value<long>(),
            JTokenType.Float => token.Value<double>(),
            JTokenType.Boolean => token.Value<bool>(),
            JTokenType.String => token.Value<string>(),
            JTokenType.Guid => token.Value<Guid>(),
            JTokenType.Uri => token.Value<Uri>(),
            JTokenType.Date => token.Value<DateTimeOffset>(),
            JTokenType.TimeSpan => token.Value<TimeSpan>(),
            JTokenType.Null => null,
            JTokenType.Undefined => null,
            _ => token.ToObject<object?>()
        };
    }

/// <summary>
/// Obsolete alias maintained for backward compatibility. Use <see cref="OutputTransformationValueConverter"/>.
/// </summary>
[Obsolete("Use OutputTransformationValueConverter instead.")]
public static class OutputFormatterValueConverter
{
    public static object? ConvertJToken(JToken? token) => OutputTransformationValueConverter.ConvertJToken(token);
    public static Dictionary<string, object?> ConvertJObject(JObject source) => OutputTransformationValueConverter.ConvertJObject(source);
    public static Dictionary<string, object?>[] ConvertJArray(JArray source) => OutputTransformationValueConverter.ConvertJArray(source);
    public static Dictionary<string, object?>[] ConvertDataSet(DataSet dataSet) => OutputTransformationValueConverter.ConvertDataSet(dataSet);
}

    /// <summary>
    /// Converts a <see cref="JObject"/> into a dictionary.
    /// </summary>
    public static Dictionary<string, object?> ConvertJObject(JObject source)
    {
        return source.Properties().ToDictionary(x => x.Name, x => ConvertJToken(x.Value));
    }

    /// <summary>
    /// Converts a <see cref="JArray"/> into an array of dictionaries.
    /// Non-object elements are wrapped under a "Value" key.
    /// </summary>
    public static Dictionary<string, object?>[] ConvertJArray(JArray source)
    {
        return source.Select(element => element is JObject jobject
                ? ConvertJObject(jobject)
                : new Dictionary<string, object?>
                {
                    ["Value"] = ConvertJToken(element)
                })
            .ToArray();
    }

    /// <summary>
    /// Converts a <see cref="DataSet"/> into an array of dictionaries.
    /// If the dataset contains multiple tables, each row dictionary includes a "TableName" key.
    /// </summary>
    public static Dictionary<string, object?>[] ConvertDataSet(DataSet dataSet)
    {
        if (dataSet.Tables.Count == 0)
            return [];

        var includeTableName = dataSet.Tables.Count > 1;
        var results = new List<Dictionary<string, object?>>();

        foreach (DataTable table in dataSet.Tables)
        {
            foreach (DataRow row in table.Rows)
            {
                var dictionary = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

                foreach (DataColumn column in table.Columns)
                {
                    var value = row[column];
                    dictionary[column.ColumnName] = value == DBNull.Value ? null : value;
                }

                if (includeTableName)
                    dictionary["TableName"] = table.TableName;

                results.Add(dictionary);
            }
        }

        return results.ToArray();
    }
}