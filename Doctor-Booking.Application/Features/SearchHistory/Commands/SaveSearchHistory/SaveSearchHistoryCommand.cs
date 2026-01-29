using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.SearchHistory.Commands.SaveSearchHistory
{
    public record SaveSearchHistoryCommand : IRequest<ResponseViewModel<object>>
    {
        public int? PatientId { get; init; }
        public int? DoctorId { get; init; }
        public string? Query { get; init; }
        public string? Specialty { get; init; }
        public string? Location { get; init; }
    }
}
