using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.Payment.Command;

public record CancelPaymentCommand(int BookingId)
	: IRequest<ResponseViewModel<bool>>;
