using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using OrchardHUN.TrainingDemo.Models;

namespace OrchardHUN.TrainingDemo.Drivers
{
    // This is going to be quite similiar to PersonListPartDriver, however there are slight differences.
    [OrchardFeature("OrchardHUN.TrainingDemo.Contents")]
    public class YouTubeEmbedFieldDriver : ContentFieldDriver<YouTubeEmbedField>
    {
        public YouTubeEmbedFieldDriver()
        {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }


        // The prefix is for similar use as with parts.
        private static string GetPrefix(ContentField field, ContentPart part)
        {
            return part.PartDefinition.Name + "." + field.Name;
        }

        // Differentiator is a must, when you're attaching multiple instances of the same field to a part (or the ghost-part of a type);
        // this will make those fields distinguishable: the unique name of the field will be generated from the name of the field and
        // the name of the part it is attached to.
        private static string GetDifferentiator(ContentField field, ContentPart part)
        {
            return field.Name;
        }

        // The other difference is that the methods of a field's driver receive an additional parameter, which is the field itself.
        // We still need the part that the field is attached to.
        protected override DriverResult Display(ContentPart part, YouTubeEmbedField field, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Fields_YouTubeEmbed",
                GetDifferentiator(field, part),
                () => shapeHelper.Fields_YouTubeEmbed());
        }

        protected override DriverResult Editor(ContentPart part, YouTubeEmbedField field, dynamic shapeHelper)
        {
            return ContentShape("Fields_YouTubeEmbed_Edit",
                GetDifferentiator(field, part),
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Fields.YouTubeEmbed.Edit",
                    Model: field,
                    Prefix: GetPrefix(field, part)));
        }

        protected override DriverResult Editor(ContentPart part, YouTubeEmbedField field, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(field, GetPrefix(field, part), null, null);
            return Editor(part, field, shapeHelper);
        }

        protected override void Importing(ContentPart part, YouTubeEmbedField field, ImportContentContext context)
        {
            context.ImportAttribute(field.FieldDefinition.Name + "." + field.Name, "VideoID", v => field.VideoID = v);
        }

        protected override void Exporting(ContentPart part, YouTubeEmbedField field, ExportContentContext context)
        {
            context.Element(field.FieldDefinition.Name + "." + field.Name).SetAttributeValue("VideoID", field.VideoID);
        }

        protected override void Describe(DescribeMembersContext context)
        {
            context.Member("VideoID", typeof(string), T("Video ID"), T("The ID of the video."));
        }

        // NEXT STATION: We've already been at Placement.info (check it out to see the placement of the editor and display shapes),
        // so let's take a look at the editor shape first: GOTO Views/EditorTemplates/Fields.YouTubeEmbed.Edit.cshtml!
    }
}