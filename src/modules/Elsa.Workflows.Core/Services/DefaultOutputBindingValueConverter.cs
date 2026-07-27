namespace Elsa.Workflows;

/// <summary>
/// Default no-op converter for variable-bound output assignments.
/// </summary>
public class DefaultOutputBindingValueConverter : IOutputBindingValueConverter
{
    /// <inheritdoc />
    public object? Convert(OutputBindingValueConverterContext context) => context.Value;
}
