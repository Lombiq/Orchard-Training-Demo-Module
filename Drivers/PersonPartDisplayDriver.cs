using System.Threading.Tasks;
using Lombiq.TrainingDemo.Models;
using Lombiq.TrainingDemo.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;

namespace Lombiq.TrainingDemo.Drivers
{
    public class PersonPartDisplayDriver : ContentPartDisplayDriver<PersonPart>
    {
        public override IDisplayResult Display(PersonPart part)
        {
            return View(nameof(PersonPart), part).Location("Content: 1");
        }

        public override IDisplayResult Edit(PersonPart personPart)
        {
            return Initialize<PersonPartViewModel>("PersonPart_Edit", model =>
            {
                model.PersonPart = personPart;
                
                model.BirthDateUtc = personPart.BirthDateUtc;
                model.Name = personPart.Name;
                model.Sex = personPart.Sex;
            });
        }

        public override async Task<IDisplayResult> UpdateAsync(PersonPart model, IUpdateModel updater)
        {
            var viewModel = new PersonPartViewModel();

            await updater.TryUpdateModelAsync(viewModel, Prefix);
            
            model.BirthDateUtc = viewModel.BirthDateUtc;
            model.Name = viewModel.Name;
            model.Sex = viewModel.Sex;
            
            return Edit(model);
        }
    }
}
