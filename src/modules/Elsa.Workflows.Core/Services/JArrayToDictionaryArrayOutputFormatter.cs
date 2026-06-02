using Elsa.Workflows.Helpers;
using Elsa.Workflows.Models;
using Newtonsoft.Json.Linq;

namespace Elsa.Workflows;

/// <summary>
/// Transforms <see cref="JArray"/> values into arrays of dictionaries.
/// </summary>
public class JArrayToDictionaryArrayOutputTransformation : IActivityOutputTransformation
{
    /// <inheritdoc />
    public string Name => "JArrayToDictionaryArray";

    /// <inheritdoc />
    public string DisplayName => "JArray to Dictionary Array";

    /// <inheritdoc />
    public string? Category => "JavaScript";

    /// <inheritdoc />
    public ActivityOutputTransformationDescriptor GetDescriptor() => new(Name, DisplayName, typeof(JArray), typeof(Dictionary<string, object?>[]), Category);

    /// <inheritdoc />
    public ValueTask<bool> CanTransformAsync(ActivityOutputTransformationResolutionContext context) => new(context.Value is JArray);

    /// <inheritdoc />
    public ValueTask<object?> TransformAsync(ActivityOutputTransformationContext context)
    {
        var jarray = context.Value as JArray;
        return new(jarray != null ? OutputTransformationValueConverter.ConvertJArray(jarray) : null);
    }
}