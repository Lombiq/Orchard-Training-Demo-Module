/*
 * IndexHandlers are different from IndexProviders. While IndexProviders will store indexed values in the SQL database
 * to index documents, IndexHandlers will use a text search index provider (e.g. Lucene) to index data. This way the
 * text search will be executed by the indexing service.
 */

using Lombiq.TrainingDemo.Fields;
using OrchardCore.Indexing;
using System.Threading.Tasks;

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
                // The color name will be indexed. Keys identify a piece of text in the index document of a given
                // content item. So for example two fields (named differently of course) will have different keys.
                context.DocumentIndex.Set(key, field.ColorName, options);
            }

            return Task.CompletedTask;
        }
    }
}

// END OF TRAINING SECTION: Indexing Content Fields in Lucene

// NEXT STATION: Views/ColorField.Option.cshtml