using System.Data;
using Elsa.Workflows.Helpers;
using Elsa.Workflows.Models;

namespace Elsa.Workflows;

/// <summary>
/// Transforms <see cref="DataSet"/> values into arrays of dictionaries.
/// </summary>
public class DataSetToDictionaryArrayOutputTransformation : IActivityOutputTransformation
{
    /// <inheritdoc />
    public string Name => "DataSetToDictionaryArray";

    /// <inheritdoc />
    public string DisplayName => "DataSet to Dictionary Array";

    /// <inheritdoc />
    public string? Category => "JavaScript";

    /// <inheritdoc />
    public ActivityOutputTransformationDescriptor GetDescriptor() => new(Name, DisplayName, typeof(DataSet), typeof(Dictionary<string, object?>[]), Category);

    /// <inheritdoc />
    public ValueTask<bool> CanTransformAsync(ActivityOutputTransformationResolutionContext context) => new(context.Value is DataSet);

    /// <inheritdoc />
    public ValueTask<object?> TransformAsync(ActivityOutputTransformationContext context)
    {
        var dataSet = context.Value as DataSet;
        return new(dataSet != null ? OutputTransformationValueConverter.ConvertDataSet(dataSet) : null);
    }
}