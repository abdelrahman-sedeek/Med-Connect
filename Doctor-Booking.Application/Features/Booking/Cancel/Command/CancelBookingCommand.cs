using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.Booking.Cancel.Query;

public record CancelBookingCommand(int BookingId)
	: IRequest<ResponseViewModel<bool>>;
