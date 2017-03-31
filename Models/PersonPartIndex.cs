using YesSql.Core.Indexes;

namespace OrchardHUN.TrainingDemo.Models
{
    public class PersonPartIndex : MapIndex
    {
        public string ContentItemId { get; set; }

        public string Name { get; set; }
    }

    public class PersonPartIndexProvider : IndexProvider<PersonPart>
    {
        public override void Describe(DescribeContext<PersonPart> context)
        {
            context.For<PersonPartIndex>()
                .Map(person =>
                {
                    var personName = person?.Name.Text;
                    var contentItemId = person?.ContentItem.ContentItemId;
                    if (!string.IsNullOrEmpty(personName))
                    {
                        return new PersonPartIndex
                        {
                            ContentItemId = contentItemId,
                            Name = personName
                        };
                    }

                    return null;
                });
        }
    }
}
