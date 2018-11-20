using System.Threading.Tasks;
using Lombiq.TrainingDemo.Fields;
using OrchardCore.Indexing;

namespace Lombiq.TrainingDemo.Indexing
{
    public class ColorFieldIndexHandler : ContentFieldIndexHandler<ColorField>
    {
        public override Task BuildIndexAsync(ColorField field, BuildFieldIndexContext context)
        {
            var options = context.Settings.ToOptions();

            foreach (var key in context.Keys)
            {
                context.DocumentIndex.Set(key, field.ColorName, options);
            }

            return Task.CompletedTask;
        }
    }
}