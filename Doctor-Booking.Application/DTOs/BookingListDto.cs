using Doctor_Booking.Domain.Entities;
using Doctor_Booking.Domain.Enum;

namespace Doctor_Booking.Application.DTOs;

public class BookingListDto
{
	public int BookingId { get; set; }
	public string DocName { get; set; }
	public string PatientName { get; set; }
	public DateTime Appointment {  get; set; }
	public BookingStatus status { get; set; }
}
