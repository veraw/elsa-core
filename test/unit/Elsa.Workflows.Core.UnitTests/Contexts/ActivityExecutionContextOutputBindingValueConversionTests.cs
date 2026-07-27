using Elsa.Expressions.Models;
using Elsa.Extensions;
using Elsa.Testing.Shared;
using Elsa.Workflows.Activities;
using Elsa.Workflows.Memory;
using Elsa.Workflows.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Workflows.Core.UnitTests.Contexts;

public class ActivityExecutionContextOutputBindingValueConversionTests
{
    [Fact]
    public async Task Set_AppliesConverter_WhenOutputIsBoundToVariable()
    {
        // Arrange
        var tracker = new ConversionTracker();
        var variable = new Variable<int>("result", 0);
        var activity = new OutputWriterActivity
        {
            Value = "42",
            Result = new Output<string>(variable)
        };

        var fixture = new ActivityTestFixture(activity)
            .ConfigureServices(services =>
            {
                services.AddSingleton(tracker);
                services.AddScoped<IOutputBindingValueConverter, TestOutputBindingValueConverter>();
            })
            .ConfigureContext(context => context.WorkflowExecutionContext.MemoryRegister.Declare(variable));

        // Act
        var context = await fixture.ExecuteAsync();

        // Assert
        Assert.Equal(1, tracker.InvocationCount);
        Assert.Equal(42, context.ExpressionExecutionContext.Get<int>(variable));
    }

    [Fact]
    public async Task Set_DoesNotApplyConverter_WhenOutputIsNotBoundToVariable()
    {
        // Arrange
        var tracker = new ConversionTracker();
        var memoryReference = new MemoryBlockReference("activityResult");
        var activity = new OutputWriterActivity
        {
            Value = "42",
            Result = new Output<string>(memoryReference)
        };

        var fixture = new ActivityTestFixture(activity)
            .ConfigureServices(services =>
            {
                services.AddSingleton(tracker);
                services.AddScoped<IOutputBindingValueConverter, TestOutputBindingValueConverter>();
            });

        // Act
        var context = await fixture.ExecuteAsync();

        // Assert
        Assert.Equal(0, tracker.InvocationCount);
        Assert.Equal("42", context.ExpressionExecutionContext.Get(memoryReference));
    }

    [Fact]
    public async Task Set_UsesOriginalValue_WhenNoConverterIsRegistered()
    {
        // Arrange
        var variable = new Variable<string>("result", string.Empty);
        var activity = new OutputWriterActivity
        {
            Value = "native-value",
            Result = new Output<string>(variable)
        };
        var fixture = new ActivityTestFixture(activity);
        fixture.ConfigureContext(context => context.WorkflowExecutionContext.MemoryRegister.Declare(variable));

        // Act
        var context = await fixture.ExecuteAsync();

        // Assert
        Assert.Equal("native-value", context.ExpressionExecutionContext.Get(variable));
    }

    private class OutputWriterActivity : CodeActivity
    {
        public string? Value { get; set; }
        public Output<string> Result { get; set; } = new();

        protected override void Execute(ActivityExecutionContext context)
        {
            context.Set(Result, Value, nameof(Result));
        }
    }

    private class TestOutputBindingValueConverter(ConversionTracker tracker) : IOutputBindingValueConverter
    {
        public object? Convert(OutputBindingValueConverterContext context)
        {
            tracker.InvocationCount++;

            if (context.TargetVariable.GetVariableType() == typeof(int) && context.Value is string stringValue)
                return int.Parse(stringValue);

            return context.Value;
        }
    }

    private class ConversionTracker
    {
        public int InvocationCount { get; set; }
    }
}
