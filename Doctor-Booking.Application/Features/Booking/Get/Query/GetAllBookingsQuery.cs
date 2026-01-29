using Doctor_Booking.Application.Common.Models;
using Doctor_Booking.Application.DTOs;
using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.Booking.Get.Query;

public record GetAllBookingsQuery
	: PaginatedRequest,
	  IRequest<ResponseViewModel<PaginatedList<BookingListDto>>>;

