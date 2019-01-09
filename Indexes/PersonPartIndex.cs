using System;
using Lombiq.TrainingDemo.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Lombiq.TrainingDemo.Indexes
{
    // This is also very similar to the one we've seen in the BookIndex.cs. The difference is that we have ContentItems
    // now instead of simple objects.
    public class PersonPartIndex : MapIndex
    {
        // Here we will reference the ContentItem ID.
        public string ContentItemId { get; set; }

        // Store the birth date only for demonstration purposes.
        public DateTime? BirthDateUtc { get; set; }
    }

    public class PersonPartIndexProvider : IndexProvider<ContentItem>
    {
        // Notice that ContentItem is what we are describing the provider for not the part.
        public override void Describe(DescribeContext<ContentItem> context)
        {
            context.For<PersonPartIndex>()
                .Map(contentItem =>
                {
                    var personPart = contentItem.As<PersonPart>();
                    if (personPart != null)
                    {
                        return new PersonPartIndex
                        {
                            ContentItemId = contentItem.ContentItemId,
                            BirthDateUtc = personPart.BirthDateUtc,
                        };
                    }

                    return null;
                });
        }
    }

    // NEXT STATION: Controllers/PersonListController
}