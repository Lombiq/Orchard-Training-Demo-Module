/*
 * We'll create our own content part now. This will be very simple: we'll use it to display the list of persons, filtered by their sex.
 * It's OK to place the part and the part record into the same code file if neither of them are particularly big.
 * Make sure to read the corresponding documentation:
 * http://docs.orchardproject.net/Documentation/Creating-a-module-with-a-simple-text-editor
 * http://docs.orchardproject.net/Documentation/Writing-a-content-part
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;
using Orchard.Core.Common.Utilities;
using Orchard.Environment.Extensions;

namespace OrchardHUN.TrainingDemo.Models
{
    /*
     * We most of the time need to use the OrchardFeature attribute if we have multiple features in a module.
     * The attribute tells Orchard that the marked class belongs to a feature, so don't use it if the feature isn't enabled. This is especially
     * important for migrations: if you wouldn't mark a subfeature's migration with this attribute it would get run when other features of the
     * module is enabled, e.g. creating tables and content types. This would be really bad...
     * The argument needed for the attribute is the feature ID given in the Module.txt.
     */
    [OrchardFeature("OrchardHUN.TrainingDemo.Contents")]
    /* Notice that a part should derive from ContentPart or ContentPart<TRecord>. We now use the latter one. This means that our part uses a record
     * to store data. This however is not mandatory: e.g. you could have a part that fetches data from some other source (like if this part would 
     * only contain that LazyField) but doesn't store anyting. Then you wouldn't need a record, so you could let the part derive from ContentPart.
     */
    public class PersonListPart : ContentPart<PersonListPartRecord>
    {
        // Normally we expose most of the properties of the record in the part too.
        // Making it required will make sure a user can't save the part without filling out the property.
        [Required]
        public Sex Sex
        {
            get { return Record.Sex; }
            set { Record.Sex = value; }
        }

        [Required]
        public int MaxCount
        {
            get { return Record.MaxCount; }
            set { Record.MaxCount = value; }
        }

        /*
         * LazyField<T> is something special. We use it to load some data into the part from an external source lazily. So if you want some
         * data be available in the part that's not stored in the corresponding record you should use LazyField.
         * LazyFields are filled from the outside. This is also needed because parts (just as records) should have a parameterless ctor: this
         * means they can't use dependencies to fetch data themselves.
         * We'll fill this field with data from a handler in a later step.
         */
        private readonly LazyField<IEnumerable<PersonRecord>> _persons = new LazyField<IEnumerable<PersonRecord>>();
        public LazyField<IEnumerable<PersonRecord>> PersonsField { get { return _persons; } }
        public IEnumerable<PersonRecord> Persons
        {
            get { return _persons.Value; }
        }
    }

    /*
     * A part record should derive from ContentPartRecord but otherwise the same rules apply as for normal records (see PersonRecord).
     * Here we write a ContentPart, meaning that the part won't get versioned. We could also write a versionable part that's data is stored
     * separately every time the corresponding content item is published. For this we would derive the record from ContentPartVersionRecord.
     * Otherwise only a change in migrations is necessary (you'll see it there) and the part is automatically versioned.
     */
    [OrchardFeature("OrchardHUN.TrainingDemo.Contents")]
    public class PersonListPartRecord : ContentPartRecord
	{
        public virtual Sex Sex { get; set; }
        public virtual int MaxCount { get; set; }

        public PersonListPartRecord()
        {
            MaxCount = 10;
        }
	}

    // NEXT STATION: ContentsMigrations
}