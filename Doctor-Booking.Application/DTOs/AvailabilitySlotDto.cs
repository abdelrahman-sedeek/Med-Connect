
namespace Doctor_Booking.Application.DTOs;

public record AvailabilitySlotDto(
	int Id,
	DateTime StartTime,
	DateTime EndTime,
	bool IsBooked
);
