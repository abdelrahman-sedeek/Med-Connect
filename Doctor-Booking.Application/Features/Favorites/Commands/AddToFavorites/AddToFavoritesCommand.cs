using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.Favorites.Commands.AddToFavorites
{
    public record AddToFavoritesCommand : IRequest<ResponseViewModel<object>>
    {
        public int PatientId { get; init; }
        public int DoctorId { get; init; }

        public AddToFavoritesCommand(int patientId, int doctorId)
        {
            PatientId = patientId;
            DoctorId = doctorId;
        }
    }
}
