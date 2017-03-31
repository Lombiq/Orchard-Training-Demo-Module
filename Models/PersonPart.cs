using Orchard.ContentFields.Fields;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Metadata.Models;
using Orchard.ContentManagement.MetaData;
using System;
using System.Linq;

namespace OrchardHUN.TrainingDemo.Models
{
    public class PersonPart : ContentPart
    {
        public TextField Name { get; set; }
        //public TextField Name =>
        //    this.AsField<TextField>(nameof(PersonPart), "Name");

        public Sex Sex { get; set; }

        public LinkField ProfessionalProfile { get; set; }

        public DateTime? BirthDateUtc { get; set; }

        public TextField Biography { get; set; }

        //public T AsField<T>(this IContent content, string partname, string fieldname)
        //    where T : ContentField
        //{
        //    //this is so the behaviour is consistent with orchard.contentmanagement.contentextensions.as<> ().
        //    if (content == null) return default(T);

        //    var contenttypedefinition = _contentdefinitionmanager.gettypedefinition(partname);
        //    var contentpartdefinition = _contentdefinitionmanager.getpartdefinition(partname);
        //    return (T)contentpartdefinition.fields.where(field => field.name == fieldname)
        //        .singleordefault();

            
        //}
    }


    public enum Sex
    {
        Male,
        Female
    }
}

// NEXT STATION: Migrations/PersonMigration