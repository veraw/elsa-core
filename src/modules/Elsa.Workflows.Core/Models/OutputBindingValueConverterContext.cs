using Elsa.Workflows.Memory;
using Elsa.Workflows.Models;

namespace Elsa.Workflows;

/// <summary>
/// Provides context for converting values assigned from activity outputs to bound variables.
/// </summary>
public record OutputBindingValueConverterContext(
    ActivityExecutionContext ActivityExecutionContext,
    Output Output,
    string OutputName,
    Variable TargetVariable,
    object? Value);
