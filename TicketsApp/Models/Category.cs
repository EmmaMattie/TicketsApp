namespace TicketsApp.Models
{
    // Category model
    public class Category
    {
        public int CategoryId { get; set; }     // Primary key
        public string Title { get; set; }       // Category name

        // One category has many events
        public List<Event> Events { get; set; } = new List<Event>();
    }
}
