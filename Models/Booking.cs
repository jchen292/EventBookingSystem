using System.ComponentModel.DataAnnotations;

namespace EventBookingSystem.Models
{
    public class Booking
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public Event Event { get; set; } = null!;

    public string UserId { get; set; } = string.Empty;

    public DateTime BookingDate { get; set; } = DateTime.Now;
}
}