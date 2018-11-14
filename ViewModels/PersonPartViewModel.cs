using System;
using System.Collections.Generic;
using System.Text;
using Lombiq.TrainingDemo.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Lombiq.TrainingDemo.ViewModels
{
    public class PersonPartViewModel
    {
        public string Name { get; set; }
        public Sex Sex { get; set; }
        public DateTime? BirthDateUtc { get; set; }
        public string Biography { get; set; }

        [BindNever]
        public PersonPart PersonPart { get; set; }
    }
}
