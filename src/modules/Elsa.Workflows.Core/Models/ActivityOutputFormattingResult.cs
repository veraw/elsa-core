namespace Elsa.Workflows.Models;

/// <summary>
/// Represents the result of a transformation attempt.
/// </summary>
public record ActivityOutputTransformationResult(
    bool Success,
    object? Value,
    string? TransformationName = null,
    Exception? Exception = null)
{
    /// <summary>
    /// Creates a successful result.
    /// </summary>
    public static ActivityOutputTransformationResult FromSuccess(object? value, string? transformationName) => new(true, value, transformationName);

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    public static ActivityOutputTransformationResult FromFailure(Exception exception) => new(false, null, null, exception);
}