namespace TicketsApp.Models
{
    // Category model represents a ticket category in the system
    public class Category
    {
        public int CategoryId { get; set; }     // Primary key for Category
        public string Title { get; set; }       // Name of the Category (e.g., Art Show, Concert)

        // Navigation property: One Category can have many Events
        public List<Event> Events { get; set; } = new List<Event>(); // Initializes an empty list for related events
    }
}
