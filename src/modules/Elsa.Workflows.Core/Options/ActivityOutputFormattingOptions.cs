namespace Elsa.Workflows.Options;

/// <summary>
/// Controls output transformation behavior.
/// </summary>
public class ActivityOutputTransformationOptions
{
    /// <summary>
    /// Determines how transformation errors are handled.
    /// </summary>
    public ActivityOutputTransformationFailureMode FailureMode { get; set; } = ActivityOutputTransformationFailureMode.Throw;
}

/// <summary>
/// Specifies how transformation failures are handled.
/// </summary>
public enum ActivityOutputTransformationFailureMode
{
    /// <summary>
    /// Throw an exception when transformation fails.
    /// </summary>
    Throw,

    /// <summary>
    /// Fall back to the native output value.
    /// </summary>
    FallbackToNative,

    /// <summary>
    /// Skip setting the transformed output.
    /// </summary>
    SkipFormattedOutput
}