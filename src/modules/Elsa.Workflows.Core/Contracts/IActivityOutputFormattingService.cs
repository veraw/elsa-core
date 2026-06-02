using Elsa.Workflows.Models;

namespace Elsa.Workflows;

/// <summary>
/// Orchestrates output transformations at runtime.
/// </summary>
public interface IActivityOutputTransformationService
{
    /// <summary>
    /// Attempts to transform the provided output value according to descriptor metadata and runtime policy.
    /// </summary>
    ValueTask<ActivityOutputTransformationResult> TryTransformAsync(ActivityOutputTransformationRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Computes the transformed output alias name for the specified output descriptor.
    /// </summary>
    string GetTransformedOutputName(OutputDescriptor outputDescriptor);
}