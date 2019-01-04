using System.ComponentModel.DataAnnotations;

namespace Lombiq.TrainingDemo.ViewModels
{
    public class BookViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        public string Description { get; set; }
    }
}