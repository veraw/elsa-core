using Elsa.Workflows.Models;
using Elsa.Workflows.Options;
using Microsoft.Extensions.Options;

namespace Elsa.Workflows.Core.UnitTests.Services;

public class ActivityOutputTransformationServiceTests
{
    [Fact]
    public async Task Uses_DefaultTransformation_From_OutputDescriptor()
    {
        var transformation = new TestTransformation("Json", canTransform: true, transformedValue: "transformed");
        var resolver = new ActivityOutputTransformationResolver([transformation]);
        var service = new ActivityOutputTransformationService(resolver, Microsoft.Extensions.Options.Options.Create(new ActivityOutputTransformationOptions()));
        var descriptor = CreateDescriptor(defaultTransformation: "Json");
        var request = new ActivityOutputTransformationRequest(null!, descriptor, new { Value = 1 });

        var result = await service.TryTransformAsync(request);

        Assert.True(result.Success);
        Assert.Equal("transformed", result.Value);
        Assert.Equal("Json", result.TransformationName);
    }

    [Fact]
    public async Task Fallbacks_To_Native_Value_When_Configured()
    {
        var resolver = new ActivityOutputTransformationResolver([]);
        var options = Microsoft.Extensions.Options.Options.Create(new ActivityOutputTransformationOptions { FailureMode = ActivityOutputTransformationFailureMode.FallbackToNative });
        var service = new ActivityOutputTransformationService(resolver, options);
        var descriptor = CreateDescriptor(defaultTransformation: "Missing");
        var request = new ActivityOutputTransformationRequest(null!, descriptor, 42);

        var result = await service.TryTransformAsync(request);

        Assert.True(result.Success);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public async Task Skips_Formatted_Output_When_Configured()
    {
        var resolver = new ActivityOutputTransformationResolver([]);
        var options = Microsoft.Extensions.Options.Options.Create(new ActivityOutputTransformationOptions { FailureMode = ActivityOutputTransformationFailureMode.SkipFormattedOutput });
        var service = new ActivityOutputTransformationService(resolver, options);
        var descriptor = CreateDescriptor(defaultTransformation: "Missing");
        var request = new ActivityOutputTransformationRequest(null!, descriptor, 42);

        var result = await service.TryTransformAsync(request);

        Assert.False(result.Success);
        Assert.NotNull(result.Exception);
    }

    [Fact]
    public void Returns_Default_Transformed_Alias_Name()
    {
        var resolver = new ActivityOutputTransformationResolver([]);
        var service = new ActivityOutputTransformationService(resolver, Microsoft.Extensions.Options.Options.Create(new ActivityOutputTransformationOptions()));
        var descriptor = CreateDescriptor(name: "Result", transformedOutputName: null);

        var alias = service.GetTransformedOutputName(descriptor);

        Assert.Equal("ResultTransformed", alias);
    }

    private static OutputDescriptor CreateDescriptor(string name = "Result", string? defaultTransformation = null, string? transformedOutputName = null)
    {
        return new OutputDescriptor(
            name,
            name,
            typeof(object),
            _ => null,
            (_, _) => { },
            defaultTransformation: defaultTransformation,
            transformedOutputName: transformedOutputName);
    }

    private class TestTransformation(string name, bool canTransform, object? transformedValue) : IActivityOutputTransformation
    {
        public string Name { get; } = name;
        public string DisplayName => Name;
        public string? Category => null;

        public ActivityOutputTransformationDescriptor GetDescriptor() => new(Name, DisplayName, typeof(object), typeof(object), Category);

        public ValueTask<bool> CanTransformAsync(ActivityOutputTransformationResolutionContext context) => new(canTransform);

        public ValueTask<object?> TransformAsync(ActivityOutputTransformationContext context) => new(transformedValue);
    }
}