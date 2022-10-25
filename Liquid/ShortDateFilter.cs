using Fluid;
using Fluid.Values;
using OrchardCore.Liquid;
using System;
using System.Threading.Tasks;

namespace Lombiq.TrainingDemo.Liquid;

// This liquid filter can be used like {{ Model.ContentItem.CreatedUtc | short_date }}
// this will render the CreatedUtc in short date format.

public class ShortDateFilter : ILiquidFilter
{
    public ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, LiquidTemplateContext context)
    {
        if (input.ToObjectValue() is not DateTime dateInput) return input;

        return new StringValue(dateInput.ToShortDateString());
    }
}
