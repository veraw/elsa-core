using Elsa.Workflows.Models;
using Elsa.Workflows.Options;
using Microsoft.Extensions.Options;

namespace Elsa.Workflows;

/// <inheritdoc />
public class ActivityOutputTransformationService(IActivityOutputTransformationResolver transformationResolver, IOptions<ActivityOutputTransformationOptions> options) : IActivityOutputTransformationService
{
    /// <inheritdoc />
    public string GetTransformedOutputName(OutputDescriptor outputDescriptor)
    {
        return string.IsNullOrWhiteSpace(outputDescriptor.TransformedOutputName) ? $"{outputDescriptor.Name}Transformed" : outputDescriptor.TransformedOutputName;
    }

    /// <inheritdoc />
    public async ValueTask<ActivityOutputTransformationResult> TryTransformAsync(ActivityOutputTransformationRequest request, CancellationToken cancellationToken = default)
    {
        var descriptor = request.OutputDescriptor;
        var transformationName = request.TransformationName ?? descriptor.DefaultTransformation;
        var category = request.Category ?? descriptor.TransformationCategory;
        var resolutionContext = new ActivityOutputTransformationResolutionContext(request.ActivityExecutionContext, descriptor, request.Value, transformationName, category);
        var transformation = await transformationResolver.ResolveAsync(resolutionContext, cancellationToken);

        if (transformation == null)
            return HandleFailure(new InvalidOperationException($"No output formatter could be resolved for output '{descriptor.Name}'."), request.Value);

        try
        {
            var transformationDescriptor = transformation.GetDescriptor();
            var transformationContext = new ActivityOutputTransformationContext(request.ActivityExecutionContext, descriptor, request.Value, transformationDescriptor);
            var transformedValue = await transformation.TransformAsync(transformationContext);
            return ActivityOutputTransformationResult.FromSuccess(transformedValue, transformationDescriptor.Name);
        }
        catch (Exception e)
        {
            return HandleFailure(e, request.Value);
        }
    }

    private ActivityOutputTransformationResult HandleFailure(Exception exception, object? fallbackValue)
    {
        return options.Value.FailureMode switch
        {
            ActivityOutputTransformationFailureMode.Throw => throw exception,
            ActivityOutputTransformationFailureMode.FallbackToNative => ActivityOutputTransformationResult.FromSuccess(fallbackValue, null),
            ActivityOutputTransformationFailureMode.SkipFormattedOutput => ActivityOutputTransformationResult.FromFailure(exception),
            _ => throw exception
        };
    }
}