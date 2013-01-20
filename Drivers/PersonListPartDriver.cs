/*
 * Make sure you understand what drivers are: http://docs.orchardproject.net/Documentation/Content-types
 * Basically all the drivers of a content type's parts are called when building the display or editor shape of a content item (and also when
 * importing/exporting).
 */

using System;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;
using OrchardHUN.TrainingDemo.Models;

namespace OrchardHUN.TrainingDemo.Drivers
{
    [OrchardFeature("OrchardHUN.TrainingDemo.Contents")]
    public class PersonListPartDriver : ContentPartDriver<PersonListPart>
    {
        // This prefix will be used to distinguish between similarly named input fields when building the editor form
        protected override string Prefix
        {
            get { return "OrchardHUN.TrainingDemo.Contents.PersonListPart"; }
        }

        // This method gets called when building the display shape of the content item the part is attached to.
        protected override DriverResult Display(PersonListPart part, string displayType, dynamic shapeHelper)
        {
            // We'll use the same shape type name specified here later in the Placement.info file.
            // The namings are conventional.
            return ContentShape("Parts_PersonList",
                // Here a display shape is built (see: http://docs.orchardproject.net/Documentation/Accessing-and-rendering-shapes).
                // The part is automatically passed to it, but we can add arbitrary data to it just as we now do with displayType.
                () => shapeHelper.Parts_PersonList(DisplayType: displayType));
        }

        // Building the editor shape.
        protected override DriverResult Editor(PersonListPart part, dynamic shapeHelper)
        {
            // Again, conventional namings.
            return ContentShape("Parts_PersonList_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts.PersonList",
                    Model: part,
                    Prefix: Prefix));
        }

        // This editor method will be called when the editor form is posted.
        protected override DriverResult Editor(PersonListPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            // We could check if the model state is valid through TryUpdateModel's return value, e.g. is Sex properly set?
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }

        protected override void Exporting(PersonListPart part, ExportContentContext context)
        {
            var element = context.Element(part.PartDefinition.Name);

            element.SetAttributeValue("Sex", part.Sex);
            element.SetAttributeValue("MaxCount", part.MaxCount);
        }

        protected override void Importing(PersonListPart part, ImportContentContext context)
        {
            var partName = part.PartDefinition.Name;

            context.ImportAttribute(partName, "Sex", value => part.Sex = (Sex)Enum.Parse(typeof(Sex), value));
            context.ImportAttribute(partName, "MaxCount", value => part.MaxCount = int.Parse(value));
        }
    }

    // NEXT STATION: We should now define where the shapes returned from here are displayed. So let's look at Placement.info
}