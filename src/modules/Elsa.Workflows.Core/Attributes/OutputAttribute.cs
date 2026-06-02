namespace Elsa.Workflows.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.ReturnValue)]
public class OutputAttribute : Attribute
{
    /// <summary>
    /// The technical name of the activity property.
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// The user-friendly name of the activity property.
    /// </summary>
    public string? DisplayName { get; set; }
        
    /// <summary>
    /// A brief description about this property for workflow tooling to use when displaying activity editors.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// A value indicating whether this property should be visible.
    /// </summary>
    public bool IsBrowsable { get; set; } = true;

    /// <summary>
    /// A value indicating whether this output can be serialized as part of the workflow instance,
    /// </summary>
    public bool IsSerializable { get; set; } = true;

    /// <summary>
    /// Optional category used to scope transformation selection.
    /// </summary>
    public string? TransformationCategory { get; set; }

    /// <summary>
    /// Optional default transformation name to use at runtime.
    /// </summary>
    public string? DefaultTransformation { get; set; }

    /// <summary>
    /// Optional output alias name for the transformed variant. Defaults to "{OutputName}Transformed".
    /// </summary>
    public string? TransformedOutputName { get; set; }

    /// <summary>
    /// A value representing options specific to a given UI hint or transformation selector.
    /// </summary>
    public object? Options { get; set; }

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

    /// <summary>
    /// A <see cref="IPropertyUIHandler"/> type that can be used to customize the UI for this property.
    /// </summary>
    public Type? UIHandler { get; set; }

    /// <summary>
    /// A set of <see cref="IPropertyUIHandler"/> types that can be used to customize the UI for this property.
    /// </summary>
    public Type[]? UIHandlers { get; set; }
}