using Elsa.Workflows.Models;

namespace Elsa.Workflows;

/// <summary>
/// Transforms activity output values into alternative representations.
/// </summary>
public interface IActivityOutputTransformation
{
    /// <summary>
    /// A unique transformation name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// A human-readable transformation name.
    /// </summary>
    string DisplayName { get; }

    /// <summary>
    /// An optional category used to scope transformation availability.
    /// </summary>
    string? Category { get; }

    /// <summary>
    /// Returns a descriptor for this transformation.
    /// </summary>
    ActivityOutputTransformationDescriptor GetDescriptor();

    /// <summary>
    /// Returns true if this transformation can transform the specified output.
    /// </summary>
    ValueTask<bool> CanTransformAsync(ActivityOutputTransformationResolutionContext context);

    /// <summary>
    /// Transforms the provided output value.
    /// </summary>
    ValueTask<object?> TransformAsync(ActivityOutputTransformationContext context);
}