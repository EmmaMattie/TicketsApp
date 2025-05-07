using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketsApp.Models
{
    // Event model
    public class Event
    {
        public int EventId { get; set; }           // Primary key
        public string Title { get; set; }          // Event title
        public string Description { get; set; }    // Event description
        public string ImageFilename { get; set; }  // Image file name
        public DateTime EventDateTime { get; set; } // Date & time
        public string Location { get; set; }       // Event location
        public decimal Price { get; set; }         // Ticket price
        public DateTime CreateDate { get; set; }   // When it was created

        public int CategoryId { get; set; }        // Foreign key
        public Category Category { get; set; } = null!; // Navigation property
    }
}
