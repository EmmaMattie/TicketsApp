using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketsApp.Models
{
    public class Event
    {
        public int EventId { get; set; }  // Primary key for the event

        [Required]  // Title is a required field
        public string Title { get; set; }  // Event's title

        [Required]  // Description is a required field
        public string Description { get; set; }  // Event's description

        [Required]  // EventDateTime is a required field
        public DateTime EventDateTime { get; set; }  // Date and time of the event

        [Required]  // Location is a required field
        public string Location { get; set; }  // Event location

        [Required]  // Price is a required field
        public decimal Price { get; set; }  // Event ticket price

        [ValidateNever]  // Don't validate the ImageFileName during model binding
        public string ImageFileName { get; set; }  // File name of the event's image

        public DateTime CreateDate { get; set; }  // Date when the event was created

        // Foreign Key to Category
        public int CategoryId { get; set; }  // ID of the category for this event

        // Navigation property to Category
        [ValidateNever]  // Don't validate Category during model binding
        public Category Category { get; set; }  // Event's associated category

        [NotMapped]  // ImageFile is not mapped to the database
        [ValidateNever]  // Don't validate ImageFile during model binding
        public IFormFile ImageFile { get; set; }  // Uploaded image for the event
    }
}
