namespace Elsa.Workflows.Models;

/// <summary>
/// Context used to resolve an activity output transformation.
/// </summary>
public record ActivityOutputTransformationResolutionContext(
    ActivityExecutionContext ActivityExecutionContext,
    OutputDescriptor OutputDescriptor,
    object? Value,
    string? TransformationName = null,
    string? Category = null);

/// <summary>
/// Obsolete alias maintained for backward compatibility. Use <see cref="ActivityOutputTransformationResolutionContext"/>.
/// </summary>
[Obsolete("Use ActivityOutputTransformationResolutionContext instead.")]
public record ActivityOutputFormatterResolutionContext(
    ActivityExecutionContext ActivityExecutionContext,
    OutputDescriptor OutputDescriptor,
    object? Value,
    string? FormatterName = null,
    string? Category = null)
    : ActivityOutputTransformationResolutionContext(ActivityExecutionContext, OutputDescriptor, Value, FormatterName, Category);