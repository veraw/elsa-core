using Elsa.Workflows.Models;

namespace Elsa.Workflows;

/// <summary>
/// Resolves output transformations for activity outputs.
/// </summary>
public interface IActivityOutputTransformationResolver
{
    /// <summary>
    /// Returns all registered transformation descriptors.
    /// </summary>
    ValueTask<IEnumerable<ActivityOutputTransformationDescriptor>> ListDescriptorsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Resolves the best matching transformation for the specified context.
    /// </summary>
    ValueTask<IActivityOutputTransformation?> ResolveAsync(ActivityOutputTransformationResolutionContext context, CancellationToken cancellationToken = default);
}