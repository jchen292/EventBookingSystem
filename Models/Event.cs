using System.ComponentModel.DataAnnotations;

namespace EventBookingSystem.Models
{
    public class Event
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public DateTime EventDate { get; set; }

    public required string Location { get; set; }

    public int Capacity { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
}