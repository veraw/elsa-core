namespace Elsa.Workflows.Models;

/// <summary>
/// Context used by transformations while transforming output values.
/// </summary>
public record ActivityOutputTransformationContext(
    ActivityExecutionContext ActivityExecutionContext,
    OutputDescriptor OutputDescriptor,
    object? Value,
    ActivityOutputTransformationDescriptor TransformationDescriptor);

/// <summary>
/// Obsolete alias maintained for backward compatibility. Use <see cref="ActivityOutputTransformationContext"/>.
/// </summary>
[Obsolete("Use ActivityOutputTransformationContext instead.")]
public record ActivityOutputFormatterContext(
    ActivityExecutionContext ActivityExecutionContext,
    OutputDescriptor OutputDescriptor,
    object? Value,
    ActivityOutputFormatterDescriptor FormatterDescriptor)
    : ActivityOutputTransformationContext(ActivityExecutionContext, OutputDescriptor, Value, FormatterDescriptor);
