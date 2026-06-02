using Elsa.Workflows;
using Elsa.Workflows.Models;

// ReSharper disable once CheckNamespace
namespace Elsa.Extensions;

/// <summary>
/// Output transformation helper APIs for activity execution.
/// </summary>
public static class ActivityExecutionContextExtensionsOutputTransformation
{
    extension(ActivityExecutionContext context)
    {
        /// <summary>
        /// Transforms the specified output value using configured output metadata.
        /// </summary>
        public async Task<ActivityOutputTransformationResult> TransformOutputAsync(string outputName, object? value, string? transformationName = default, CancellationToken cancellationToken = default)
        {
            var normalizedOutputName = ActivityOutputRegister.NormalizeOutputName(context, outputName);
            var outputDescriptor = context.ActivityDescriptor.Outputs.FirstOrDefault(x => x.Name == normalizedOutputName);

            if (outputDescriptor == null)
                throw new InvalidOperationException($"Output descriptor '{outputName}' was not found for activity '{context.Activity.Type}'.");

            var transformationService = context.GetRequiredService<IActivityOutputTransformationService>();
            var request = new ActivityOutputTransformationRequest(context, outputDescriptor, value, transformationName, outputDescriptor.TransformationCategory, IsLazyEvaluation: false);
            return await transformationService.TryTransformAsync(request, cancellationToken);
        }

        /// <summary>
        /// Transforms and stores the transformed variant for the specified output.
        /// </summary>
        public async Task<ActivityOutputTransformationResult> SetTransformedOutputAsync(string outputName, object? value, string? transformationName = default, CancellationToken cancellationToken = default)
        {
            var transformResult = await context.TransformOutputAsync(outputName, value, transformationName, cancellationToken);

            if (!transformResult.Success)
                return transformResult;

            var normalizedOutputName = ActivityOutputRegister.NormalizeOutputName(context, outputName);
            var outputDescriptor = context.ActivityDescriptor.Outputs.First(x => x.Name == normalizedOutputName);
            var transformationService = context.GetRequiredService<IActivityOutputTransformationService>();
            var transformedOutputName = transformationService.GetTransformedOutputName(outputDescriptor);
            context.WorkflowExecutionContext.RecordActivityOutput(context, transformedOutputName, transformResult.Value);

            return transformResult;
        }

        /// <summary>
        /// Obsolete alias. Use <see cref="TransformOutputAsync"/>.
        /// </summary>
        [Obsolete("Use TransformOutputAsync instead.")]
        public Task<ActivityOutputTransformationResult> FormatOutputAsync(string outputName, object? value, string? formatterName = default, CancellationToken cancellationToken = default) => context.TransformOutputAsync(outputName, value, formatterName, cancellationToken);

        /// <summary>
        /// Obsolete alias. Use <see cref="SetTransformedOutputAsync"/>.
        /// </summary>
        [Obsolete("Use SetTransformedOutputAsync instead.")]
        public Task<ActivityOutputTransformationResult> SetFormattedOutputAsync(string outputName, object? value, string? formatterName = default, CancellationToken cancellationToken = default) => context.SetTransformedOutputAsync(outputName, value, formatterName, cancellationToken);
    }
}