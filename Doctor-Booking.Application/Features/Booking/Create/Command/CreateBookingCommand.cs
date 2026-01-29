using Doctor_Booking.Application.DTOs;
using Doctor_Booking.Application.ViewModels;
using FluentValidation;
using MediatR;

namespace Doctor_Booking.Application.Features.Booking.Create.Command;


public record CreateBookingCommand( BookingDto bookingDto)
	:IRequest<ResponseViewModel<CreateBookingResult>>;

public record CreateBookingResult(int id );

public class CreateBookingValidator : AbstractValidator<CreateBookingCommand>
{
	public CreateBookingValidator()
	{
		RuleFor(x => x.bookingDto.DoctorId).NotEmpty().WithMessage("Doctor is Required");
		RuleFor(x => x.bookingDto.AvailabilitySlotId).NotEmpty().WithMessage("Date is required");


	}
}