using Elsa.Extensions;
using Elsa.Expressions.JavaScript.Extensions;
using Elsa.Expressions.JavaScript.Options;
using Elsa.Expressions.JavaScript.TypeDefinitions.Abstractions;
using Elsa.Expressions.JavaScript.TypeDefinitions.Models;
using Elsa.Workflows;
using Humanizer;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Elsa.Expressions.JavaScript.Providers;

/// <summary>
/// Produces <see cref="FunctionDefinition"/>s for common functions.
/// </summary>
[UsedImplicitly]
internal class ActivityOutputFunctionsDefinitionProvider(IActivityRegistryLookupService activityRegistryLookup, IOptions<JintOptions> options) : FunctionDefinitionProvider
{
    protected override async ValueTask<IEnumerable<FunctionDefinition>> GetFunctionDefinitionsAsync(TypeDefinitionContext context)
    {
        if(options.Value.DisableWrappers)
            return [];
        
        var nodes = context.WorkflowGraph.Nodes;
        var activitiesWithOutputs = nodes.GetActivitiesWithOutputs(activityRegistryLookup).Where(x => x.activity.Name != null && x.activity.Name.IsValidVariableName());
        var definitions = new List<FunctionDefinition>();

        await foreach (var (activity, activityDescriptor) in activitiesWithOutputs)
        {
            definitions.AddRange(from output in activityDescriptor.Outputs.Where(x => x.Name.IsValidVariableName())
                let activityNamePascalName = activity.Name!.Pascalize()
                let outputPascalName = output.Name.Pascalize()
                let formattedOutputPascalName = $"{outputPascalName}Formatted"
                from functionName in new[]
                {
                    $"get{outputPascalName}From{activityNamePascalName}",
                    $"get{formattedOutputPascalName}From{activityNamePascalName}"
                }
                select CreateFunctionDefinition(builder => builder.Name(functionName).ReturnType("any")));
        }

        return definitions;
    }
}
