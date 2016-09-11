/*
 * Make sure you understand what drivers are: http://docs.orchardproject.net/Documentation/Content-types
 * Basically all the drivers of a content type's parts are called when building the display or editor shape of a content 
 * item (and also when importing/exporting).
 * Note that the same part can have multiple drivers: e.g. you could write a driver for even TitlePart.
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
            // For the sake of demonstration we use Combined() here. It makes it possible to return multiple shapes from
            // a driver method. Use this if you'd like to return different shapes that can be used e.g. with different
            // display types.
            return Combined(
                // We'll use the same shape type name specified here later in the Placement.info file.
                // The namings are conventional.
                ContentShape("Parts_PersonList",
                // Here a display shape is built (see:
                // http://docs.orchardproject.net/Documentation/Accessing-and-rendering-shapes). The part is
                // automatically passed to it, but we can add arbitrary data to it just as we now do with displayType.
                    () =>
                    {
                        // Note that the shape is produced from this factory delegate (you can see its shorthand form at
                        // Parts_PersonList_Summary below). The reason behind using a factory is that if the shape is not
                        // displayed (because e.g. it's hidden from Placement) then its factory won't be run either. So
                        // use the factory to produce value that are costly to compute and are only needed if the shape
                        // is displayed.
                        var upperCaseDisplayType = displayType.ToUpperInvariant(); // This is a "costly" computation here :-).
                        return shapeHelper.Parts_PersonList(DisplayType: displayType, UpperDisplayType: upperCaseDisplayType);
                    }),
                // A shape for the summary: this will be only used when e.g. listing the item. See Placement.info how
                // it's used. Naming is conventional.
                ContentShape("Parts_PersonList_Summary",
                    () => shapeHelper.Parts_PersonList_Summary())
                );
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
            // We could check if the model state is valid through TryUpdateModel's return value, e.g. is Sex properly
            // set?
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }

        // Exporting and Importing is called when the content items having PersonListPart are exported or imported.
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

    // NEXT STATION: We should now define where the shapes returned from here are displayed. So let's look at
    // Placement.info!
}