using System.Reflection;
using System.Text.Json.Serialization;

namespace Elsa.Workflows.Models;

/// <summary>
/// A descriptor of an activity's output property.
/// </summary>
public class OutputDescriptor : PropertyDescriptor
{
    /// <inheritdoc />
    [JsonConstructor]
    public OutputDescriptor()
    {
    }

    /// <inheritdoc />
    public OutputDescriptor(
        string name,
        string displayName,
        Type type,
        Func<IActivity, object?> valueGetter,
        Action<IActivity, object?> valueSetter,
        PropertyInfo? propertyInfo = default,
        string? description = default,
        bool? isBrowsable = default,
        bool? isSerializable = default,
        string? transformationCategory = default,
        string? defaultTransformation = default,
        string? transformedOutputName = default,
        IDictionary<string, object>? uiSpecifications = default)
    {
        Name = name;
        DisplayName = displayName;
        Type = type;
        ValueGetter = valueGetter;
        ValueSetter = valueSetter;
        Description = description;
        IsBrowsable = isBrowsable;
        IsSerializable = isSerializable;
        TransformationCategory = transformationCategory;
        DefaultTransformation = defaultTransformation;
        TransformedOutputName = transformedOutputName;
        UISpecifications = uiSpecifications;
        PropertyInfo = propertyInfo;
    }

    /// <summary>
    /// Optional transformation category used to scope transformation selection.
    /// </summary>
    public string? TransformationCategory { get; set; }

    /// <summary>
    /// Optional default transformation name.
    /// </summary>
    public string? DefaultTransformation { get; set; }

    /// <summary>
    /// Optional transformed output alias name.
    /// </summary>
    public string? TransformedOutputName { get; set; }

    /// <summary>
    /// A dictionary of UI specifications to be used by UI tooling.
    /// </summary>
    public IDictionary<string, object>? UISpecifications { get; set; }

    /// <summary>
    /// Obsolete alias. Use <see cref="TransformationCategory"/>.
    /// </summary>
    [Obsolete("Use TransformationCategory instead.")]
    public string? FormatterCategory
    {
        get => TransformationCategory;
        set => TransformationCategory = value;
    }

    /// <summary>
    /// Obsolete alias. Use <see cref="DefaultTransformation"/>.
    /// </summary>
    [Obsolete("Use DefaultTransformation instead.")]
    public string? DefaultFormatter
    {
        get => DefaultTransformation;
        set => DefaultTransformation = value;
    }

    /// <summary>
    /// Obsolete alias. Use <see cref="TransformedOutputName"/>.
    /// </summary>
    [Obsolete("Use TransformedOutputName instead.")]
    public string? FormattedOutputName
    {
        get => TransformedOutputName;
        set => TransformedOutputName = value;
    }
}