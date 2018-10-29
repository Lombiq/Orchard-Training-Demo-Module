namespace Lombiq.TrainingDemo.Models
{
    public class Book
    {
        // Title of the book. It should be displayed on both pages.
        public string Title { get; set; }
        
        // Author of the book. It should be displayed on both pages too.
        public string Author { get; set; }

        // URL of the book cover photo. It should be displayed only on the summary page.
        public string CoverPhotoUrl { get; set; }

        // Short summary of the book. It should be displayed only on the summary page.
        public string Summary { get; set; }

        // A short sample from the book. It should be displayed only on the book sample page.
        public string Excerpt { get; set; }
    }
}

// NEXT STATION: Now go back to Controllers/BasicDisplayManagementController.