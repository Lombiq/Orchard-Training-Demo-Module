using Lombiq.TrainingDemo.Models;
using OrchardCore.ContentManagement;
using System;
using YesSql.Indexes;

namespace Lombiq.TrainingDemo.Indexes
{
    // This is also very similar to the one we've seen in BookIndex.cs. The difference is that we have ContentItems
    // now instead of simple objects.
    public class PersonPartIndex : MapIndex
    {
        // Here we will reference the ContentItem ID.
        public string ContentItemId { get; set; }

        // Store the birth date only for demonstration purposes so we can run queries on it.
        public DateTime? BirthDateUtc { get; set; }
    }

    // Don't forget to register this class with the service provider (see: Startup.cs).
    public class PersonPartIndexProvider : IndexProvider<ContentItem>
    {
        // Notice that ContentItem is what we are describing the provider for not the part.
        public override void Describe(DescribeContext<ContentItem> context) =>
            context.For<PersonPartIndex>()
                .Map(contentItem =>
                {
                    var personPart = contentItem.As<PersonPart>();

                    // Note that we can write any logic in here to determine when an index record should be created for
                    // a content item. Here we'll have a record for every content item possessing a PersonPart.
                    // However, we could e.g. only have an index for only published items (if you don't want to query
                    // on drafts; you can check contentItem.Published), or only on the latest ones (regardless of it
                    // being a draft or published version; contentItem.Latest shows this).
                    // Not cluttering up the index table with unnecessary rows can help with performance and makes
                    // managing the database easier overall.
                    // Also note that there is a lot more to index providers than just Map() (although this is what you
                    // need to use the most), see the YesSQL documentation: https://github.com/sebastienros/yessql/wiki/Tutorial
                    return personPart == null
                        ? null
                        : new PersonPartIndex
                        {
                            ContentItemId = contentItem.ContentItemId,
                            BirthDateUtc = personPart.BirthDateUtc,
                        };
                });
    }

    // END OF TRAINING SECTION: Content Item and Content Part development

    // NEXT STATION: Controllers/PersonListController
}
