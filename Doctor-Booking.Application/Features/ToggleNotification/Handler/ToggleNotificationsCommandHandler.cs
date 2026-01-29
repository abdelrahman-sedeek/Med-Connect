using Doctor_Booking.Application.Features.ToggleNotification.Command;
using Doctor_Booking.Application.Interfaces;
using Doctor_Booking.Application.ViewModels;
using Doctor_Booking.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Doctor_Booking.Application.Features.ToggleNotification.Handler;

public class ToggleNotificationsCommandHandler 
	: IRequestHandler<ToggleNotificationsCommand, ResponseViewModel<bool>>
{
	private readonly IDoctorBookingDbContext _context;

	public ToggleNotificationsCommandHandler(IDoctorBookingDbContext context)
	{
		_context = context;
	}
	public async Task<ResponseViewModel<bool>> Handle(ToggleNotificationsCommand request, CancellationToken cancellationToken)
	{

		var settings = await _context.UserSettings
			.FirstOrDefaultAsync(
				x => x.UserId == request.UserId,
				cancellationToken);

		if (settings == null)
		{
			settings = new UserSettings
			{
				UserId = request.UserId,
				NotificationsEnabled = request.Enable
			};

			_context.UserSettings.Add(settings);
		}
		else
		{
			settings.NotificationsEnabled = request.Enable;
		}

		await _context.SaveChangesAsync(cancellationToken);

		return ResponseViewModel<bool>
			.SuccessResponse(true, 200 ,"Notification settings updated successfully");

	}
}
