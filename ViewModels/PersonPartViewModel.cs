using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Lombiq.TrainingDemo.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using OrchardCore.Modules;

namespace Lombiq.TrainingDemo.ViewModels
{
    public class PersonPartViewModel : IValidatableObject
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public Sex Sex { get; set; }

        [Required]
        public DateTime? BirthDateUtc { get; set; }

        public string Biography { get; set; }

        [BindNever]
        public PersonPart PersonPart { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // To use GetService overload you need to add the Microsoft.Extensions.DependencyInjection nuget package
            // to your module.
            var T = validationContext.GetService<IStringLocalizer<PersonPartViewModel>>();
            var clock = validationContext.GetService<IClock>();

            if (BirthDateUtc.HasValue)
            {
                var age = clock.UtcNow.Year - BirthDateUtc.Value.Year;
                if (clock.UtcNow < BirthDateUtc.Value.AddYears(age)) age--;

                if (age < 18)
                {
                    yield return new ValidationResult(T["The person must be 18 or older."], new[] { nameof(BirthDateUtc) });
                }
            }
        }
    }
}