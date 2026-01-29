using Doctor_Booking.Application.Common.Notifications;
using Doctor_Booking.Application.Features.Review.Command;
using Doctor_Booking.Application.Interfaces.Services;
using Doctor_Booking.Application.Interfaces;
using Doctor_Booking.Application.ViewModels;
using Doctor_Booking.Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Doctor_Booking.Domain.Entities;

namespace Doctor_Booking.Application.Features.Review.Handler;

public class CreateReviewCommandHandler
	: IRequestHandler<CreateReviewCommand, ResponseViewModel<bool>>
{
	private readonly IDoctorBookingDbContext _context;
	private readonly INotificationService _notificationService;
	private readonly ICurrentUserService _currentUser;

	public async Task<ResponseViewModel<bool>> Handle(
		CreateReviewCommand request,
		CancellationToken cancellationToken)
	{
		var booking = await _context.Bookings
			.Include(b => b.Review)
			.FirstOrDefaultAsync(b => b.Id == request.BookingId, cancellationToken);

		if (booking == null)
			return ResponseViewModel<bool>.FailureResponse("Booking not found");

		if (booking.Status != BookingStatus.Completed)
			return ResponseViewModel<bool>.FailureResponse("Cannot review before session completion");

		if (booking.Review != null)
			return ResponseViewModel<bool>.FailureResponse("Review already submitted");

		var review = new Doctor_Booking.Domain.Entities.Review
		{
			BookingId = booking.Id,
			DoctorId = booking.DoctorId!.Value,
			PatientId = _currentUser.UserId,
			Rating = request.Rating,
			Comment = request.Comment
		};

		_context.Reviews.Add(review);
		await _context.SaveChangesAsync(cancellationToken);

		// 🔔 Notify doctor
		await _notificationService.CreateAsync(
			new NotificationMessage(
				booking.DoctorId!.Value,
				"New Review",
				"You have received a new review"
			),
			cancellationToken
		);

		return ResponseViewModel<bool>.SuccessResponse(true,200, "Review submitted successfully");
	}
}

