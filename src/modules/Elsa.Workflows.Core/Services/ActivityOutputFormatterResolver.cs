using Elsa.Workflows.Models;

namespace Elsa.Workflows;

/// <inheritdoc />
public class ActivityOutputTransformationResolver(IEnumerable<IActivityOutputTransformation> transformations) : IActivityOutputTransformationResolver
{
    private readonly IList<IActivityOutputTransformation> _transformations = transformations.ToList();

    /// <inheritdoc />
    public ValueTask<IEnumerable<ActivityOutputTransformationDescriptor>> ListDescriptorsAsync(CancellationToken cancellationToken = default)
    {
        return new(_transformations.Select(x => x.GetDescriptor()));
    }

    /// <inheritdoc />
    public async ValueTask<IActivityOutputTransformation?> ResolveAsync(ActivityOutputTransformationResolutionContext context, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(context.TransformationName))
        {
            var transformationByName = _transformations.FirstOrDefault(x => string.Equals(x.Name, context.TransformationName, StringComparison.OrdinalIgnoreCase));

            if (transformationByName == null)
                return null;

            return await transformationByName.CanTransformAsync(context) ? transformationByName : null;
        }

        var requestedCategory = context.Category;

        foreach (var transformation in _transformations)
        {
            if (!string.IsNullOrWhiteSpace(requestedCategory) && !string.Equals(transformation.Category, requestedCategory, StringComparison.OrdinalIgnoreCase))
                continue;

            if (await transformation.CanTransformAsync(context))
                return transformation;
        }

        return null;
    }
}