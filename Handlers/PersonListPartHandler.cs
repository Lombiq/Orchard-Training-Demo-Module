/*
 * We'll write a handler for our part so make sure you understand what handlers are: 
 * http://docs.orchardproject.net/Documentation/Understanding-content-handlers
 */

using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment;
using Orchard.Environment.Extensions;
using OrchardHUN.TrainingDemo.Controllers;
using OrchardHUN.TrainingDemo.Models;
using OrchardHUN.TrainingDemo.Services;
using System.Web.Routing;

namespace OrchardHUN.TrainingDemo.Handlers
{
    [OrchardFeature("OrchardHUN.TrainingDemo.Contents")]
    public class PersonListPartHandler : ContentHandler
    {
        /*
         * Injecting necessary dependencies.
         * 
         * Notice Work<T>. This construct is used to lazily inject dependencies. This means that a dependency injected 
         * with Work<T> (here IPersonManager) won't get resolved until it is really needed (i.e. when accessing the 
         * Work object's Value property). Also, Work<T> uses a different dependency lifetime scope than the scope of 
         * the request (hence the name: the scope's name is "work"). This makes it possible to resolve dependencies not 
         * just from the scope the class requesting them (e.g. an ISingletonDependency belongs to the "shell" scope but 
         * you can use IDependency types from it through Work<T>).
         * However most possibly you will only need Work<T> for lazy resolving (what can have a positive performance 
         * impact).
         * 
         * If just lazy injection is needed (i.e. only to resolve a dependency when really needed) you could also inject 
         * Lazy<T> and user it in the same way as Work<T>.
         */
        public PersonListPartHandler(
            IRepository<PersonListPartRecord> repository,
            Work<IPersonManager> personManagerWork)
        {
            // If a part stores its data in a record we need this type of plumbing for it to work.
            Filters.Add(StorageFilter.For(repository));

            // We hook into the OnActivated event of PersonListPart. Check out the other On* events.
            OnActivated<PersonListPart>((context, part) =>
            {
                // This means: when the LazyField's Value (i.e. the Persons property on our part) is first accessed
                // it will load the persons by running this lambda here. I.e. if the list of persons is not needed we
                // won't work on loading them.
                part.PersonsField.Loader(() => personManagerWork.Value.GetPersons(part.Sex, part.MaxCount));
            });
        }


        // You can do all kinds of crazy things from handlers, like overriding metadata of items as well.
        // Here we override the edit url of Person List items to point to our own controller (you'll soon see it).
        protected override void GetItemMetadata(GetContentItemMetadataContext context)
        {
            if (context.ContentItem.ContentType != "PersonList") return;

            // You'll soon see the full route decaration for this!
            context.Metadata.EditorRouteValues = new RouteValueDictionary
                                                    {
                                                        {"area", $"{nameof(OrchardHUN)}.{nameof(TrainingDemo)}"},
                                                        {"controller", "ContentsAdmin"},
                                                        {"action", $"{nameof(ContentsAdminController.PersonListDashboard)}"},
                                                        {"id", context.ContentItem.Id}
                                                    };
        }

        // NEXT STATION: Drivers/PersonListPartDriver
    }
}