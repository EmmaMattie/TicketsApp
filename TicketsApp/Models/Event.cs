using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketsApp.Models
{
    public class Event
    {
        public int EventId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime EventDateTime { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string ImageFilename { get; set; }

        public DateTime CreateDate { get; set; }

        // Foreign Key to Category
        public int CategoryId { get; set; }

        // Navigation property to Category
        public Category Category { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
