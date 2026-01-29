
namespace Doctor_Booking.Application.DTOs;

public class ReviewDto
{
	public decimal Rating { get; set; }
	public string Comment { get; set; } = null!;
	public DateTime CreatedAt { get; set; }
}
