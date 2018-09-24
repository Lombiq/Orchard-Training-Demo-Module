/*
 * Now we'll get to know how low-level DB data storage works. For this we'll create a simple record class and use it to 
 * store a person's data in a DB table.
 * We use such low-level storage if we want to store something in the DB what we don't want to use as part of content 
 * items. This includes data that e.g. our module uses internally and we don't really want to expose to the outside world.
 * Make sure to read the corresponding documentation (http://docs.orchardproject.net/Documentation/Understanding-data-access) 
 * first!
 */

using Orchard.Data.Conventions;
using System;

namespace OrchardHUN.TrainingDemo.Models
{
    /*
     * This is a record class: it's directly mapped to a DB table.
     * Be aware that all public properties should be virtual: this is needed by NHibernate, the underlying ORM library.
     * Every property declared here except Name is special in some way: you can use others (like other integer values) 
     * without further thought.
     */
    public class PersonRecord
    {
        // An integer id is needed.
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        // We can store enums as well
        public virtual Sex Sex { get; set; }

        // Best practice for DateTimes is that they should always be nullable (note the question mark).
        // Most of the time the best is to store dates in UTC.
        public virtual DateTime? BirthDateUtc { get; set; }

        // This attribute tells that the string can has infinite lenght. If we wouldn't use it the string could be truncated.
        [StringLengthMax]
        public virtual string Biography { get; set; }


        // If you want to set defaults for when some of the record's properties are not filled upon creation, set them
        // from the ctor.
        public PersonRecord() => Biography = "This person has not written a biography yet.";
    }


    public enum Sex
    {
        Male,
        Female
    }
}

// NEXT STATION: Migrations