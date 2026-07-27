using Elsa.Workflows.Memory;

namespace Elsa.Workflows;

/// <summary>
/// Converts values assigned to activity outputs that are bound to workflow variables.
/// </summary>
public interface IOutputBindingValueConverter
{
    /// <summary>
    /// Converts the output value for a variable-bound output assignment.
    /// </summary>
    object? Convert(OutputBindingValueConverterContext context);
}
