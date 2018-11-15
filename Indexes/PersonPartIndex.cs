using System;
using Lombiq.TrainingDemo.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Lombiq.TrainingDemo.Indexes
{
    public class PersonPartIndex : MapIndex
    {
        public string ContentItemId { get; set; }
        public DateTime? BirthDateUtc { get; set; }
    }

    public class PersonPartIndexProvider : IndexProvider<ContentItem>
    {
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
}