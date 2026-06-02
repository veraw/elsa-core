using Elsa.Workflows.Helpers;
using Elsa.Workflows.Models;
using Newtonsoft.Json.Linq;

namespace Elsa.Workflows;

/// <summary>
/// Transforms <see cref="JObject"/> values into dictionaries.
/// </summary>
public class JObjectToDictionaryOutputTransformation : IActivityOutputTransformation
{
    /// <inheritdoc />
    public string Name => "JObjectToDictionary";

    /// <inheritdoc />
    public string DisplayName => "JObject to Dictionary";

    /// <inheritdoc />
    public string? Category => "JavaScript";

    /// <inheritdoc />
    public ActivityOutputTransformationDescriptor GetDescriptor() => new(Name, DisplayName, typeof(JObject), typeof(Dictionary<string, object?>), Category);

    /// <inheritdoc />
    public ValueTask<bool> CanTransformAsync(ActivityOutputTransformationResolutionContext context) => new(context.Value is JObject);

    /// <inheritdoc />
    public ValueTask<object?> TransformAsync(ActivityOutputTransformationContext context)
    {
        var jobject = context.Value as JObject;
        return new(jobject != null ? OutputTransformationValueConverter.ConvertJObject(jobject) : null);
    }
}