namespace Elsa.Workflows.Models;

/// <summary>
/// Describes an activity output transformation.
/// </summary>
public record ActivityOutputTransformationDescriptor(
    string Name,
    string DisplayName,
    Type SourceType,
    Type? TargetType = null,
    string? Category = null,
    string? Description = null);

/// <summary>
/// Obsolete alias maintained for backward compatibility. Use <see cref="ActivityOutputTransformationDescriptor"/>.
/// </summary>
[Obsolete("Use ActivityOutputTransformationDescriptor instead.")]
public record ActivityOutputFormatterDescriptor(
    string Name,
    string DisplayName,
    Type SourceType,
    Type? TargetType = null,
    string? Category = null,
    string? Description = null)
    : ActivityOutputTransformationDescriptor(Name, DisplayName, SourceType, TargetType, Category, Description);