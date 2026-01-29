using Doctor_Booking.Application.Common.Notifications;
using Doctor_Booking.Application.Features.Booking.Create.Command;
using Doctor_Booking.Application.Interfaces;
using Doctor_Booking.Application.Interfaces.Services;
using Doctor_Booking.Application.ViewModels;
using Doctor_Booking.Domain.Entities;
using Doctor_Booking.Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Doctor_Booking.Application.Features.Booking.Create.Handler;

public class CreateBookingHandler :
	IRequestHandler<CreateBookingCommand, ResponseViewModel<CreateBookingResult>>
{
	private IDoctorBookingDbContext _context;
	private INotificationService _notificationService;
	private ICurrentUserService _currentUser;

	public CreateBookingHandler( IDoctorBookingDbContext context 
		, INotificationService notificationService ,
		 ICurrentUserService currentUser)
	{
		_context = context;
		_notificationService = notificationService;
		_currentUser = currentUser;
	}
	public async Task<ResponseViewModel<CreateBookingResult>> Handle
		(CreateBookingCommand request, CancellationToken cancellationToken)
	{
		var dto = request.bookingDto;

		var slot = await _context.AvailabilitySlots
		.FirstOrDefaultAsync(s => s.Id == dto.AvailabilitySlotId, cancellationToken);

		if (slot == null)
			return ResponseViewModel<CreateBookingResult>
				.FailureResponse("Availability slot not found");

		if (slot.IsBooked)
			return ResponseViewModel<CreateBookingResult>
				.FailureResponse("This time slot is already booked");

		var booking = new Domain.Entities.Booking
		{
			DoctorId = dto.DoctorId,
			PatientId = _currentUser.UserId,
			AvailabilitySlotId = slot.Id,
			Status = BookingStatus.Pending
		};

		_context.Bookings.Add(booking);
		slot.IsBooked = true;

		await _context.SaveChangesAsync(cancellationToken);
		// 🔔 Create Notification
		await _notificationService.CreateAsync(
			new NotificationMessage(
				booking.PatientId!.Value,
				"Booking Created",
				"Your booking has been created successfully"
			),
			cancellationToken
		);


		return ResponseViewModel<CreateBookingResult>.SuccessResponse(
		   new CreateBookingResult(booking.Id) );
	}
}
