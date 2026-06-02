using JetBrains.Annotations;

namespace Elsa.Api.Client.Resources.ActivityDescriptors.Models;

/// <summary>
/// A descriptor of an activity's output property.
/// </summary>
[PublicAPI]
public class OutputDescriptor : PropertyDescriptor
{
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
    /// A dictionary of UI specifications to be used by the UI.
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