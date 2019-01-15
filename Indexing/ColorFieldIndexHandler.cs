/*
 * IndexHandlers are different from IndexProviders. While IndexProviders will store values in the SQL database to index
 * documents these will use a text search index provider (e.g. Lucene) to index data. This way no database query is
 * required when you want to use a search widget on your website. 
 */

using System.Threading.Tasks;
using Lombiq.TrainingDemo.Fields;
using OrchardCore.Indexing;

namespace Lombiq.TrainingDemo.Indexing
{
    // Don't forget to register this class with the service provider (see: Startup.cs).
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