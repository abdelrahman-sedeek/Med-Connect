using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.Payment.Command;

public record RefundPaymentCommand(int BookingId)
	: IRequest<ResponseViewModel<bool>>;

