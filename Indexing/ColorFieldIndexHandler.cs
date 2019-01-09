using System.Threading.Tasks;
using Lombiq.TrainingDemo.Fields;
using OrchardCore.Indexing;

namespace Lombiq.TrainingDemo.Indexing
{
    // IndexHandlers are different from IndexProviders. While IndexProviders will store values in the SQL database to
    // index documents these will use an actual index provider (e.g. Lucene) index data. This way no database query is
    // required when you want to use a search widget on your website.
    public class ColorFieldIndexHandler : ContentFieldIndexHandler<ColorField>
    {
        public override Task BuildIndexAsync(ColorField field, BuildFieldIndexContext context)
        {
            var options = context.Settings.ToOptions();

            foreach (var key in context.Keys)
            {
                // The color name will be indexed.
                context.DocumentIndex.Set(key, field.ColorName, options);
            }

            return Task.CompletedTask;
        }
    }
}

// NEXT STATION: Views/ColorField.Option.cshtml