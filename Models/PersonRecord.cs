using System;
using System.ComponentModel.DataAnnotations;

namespace OrchardHUN.TrainingDemo.Models
{
    public class PersonRecord
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Sex Sex { get; set; }

        public DateTime? BirthDateUtc { get; set; }

        /// <summary>
        /// This attribute tells that the string can has infinite lenght.
        /// 10000 is an arbitrary number large enough to be in the nvarchar(max) range.
        /// If we wouldn't use it the string could be truncated.
        /// </summary>
        [StringLength(10000, MinimumLength = 6)]
        public string Biography { get; set; }

        public PersonRecord()
        {
            Biography = "This person has not written a biography yet.";
        }
    }

    public enum Sex
    {
        Male,
        Female
    }
}

// NEXT STATION: Migrations