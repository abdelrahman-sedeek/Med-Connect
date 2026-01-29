using Doctor_Booking.Domain.Entities;
using Doctor_Booking.Domain.Enum;

namespace Doctor_Booking.Application.DTOs;

public record BookingDto(
	
	int DoctorId ,
	//int PatientId ,
	int AvailabilitySlotId
	);
