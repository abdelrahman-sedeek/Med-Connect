using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.ToggleNotification.Command;

public record ToggleNotificationsCommand(
	int UserId,
	bool Enable
) : IRequest<ResponseViewModel<bool>>;
