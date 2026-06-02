namespace Elsa.Workflows.Models;

/// <summary>
/// Represents a request to transform an output value.
/// </summary>
public record ActivityOutputTransformationRequest(
    ActivityExecutionContext ActivityExecutionContext,
    OutputDescriptor OutputDescriptor,
    object? Value,
    string? TransformationName = null,
    string? Category = null,
    bool IsLazyEvaluation = false);