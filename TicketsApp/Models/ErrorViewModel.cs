namespace TicketsApp.Models
{
    // ErrorViewModel is used to capture error details in the application
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }  // Unique identifier for the error request (nullable)

        // ShowRequestId returns true if RequestId is not null or empty (used for displaying the request ID)
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
