using Lombiq.TrainingDemo.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using OrchardCore.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lombiq.TrainingDemo.ViewModels
{
    // IValidateObject is an ASP.NET Core feature to use on view models where the model binder will automatically
    // execute the Validate method which will return any validation error.
    public class PersonPartViewModel : IValidatableObject
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public Handedness Handedness { get; set; }

        [Required]
        public DateTime? BirthDateUtc { get; set; }

        [BindNever]
        public PersonPart PersonPart { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // To use GetService overload you need to add the Microsoft.Extensions.DependencyInjection NuGet package
            // to your module. This way you can get any service you want just as you've injected them in a constructor.
            var localizer = validationContext.GetService<IStringLocalizer<PersonPartViewModel>>();
            var clock = validationContext.GetService<IClock>();

            if (BirthDateUtc.HasValue && clock.UtcNow < BirthDateUtc.Value.AddYears(18))
            {
                yield return new ValidationResult(localizer["The person must be 18 or older."], new[] { nameof(BirthDateUtc) });
            }

            // Now go back to the PersonPartDisplayDrvier.
        }
    }
}
