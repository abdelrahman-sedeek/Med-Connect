using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.Specialties.Commands.DeleteSpecialty
{
    public record DeleteSpecialtyCommand : IRequest<ResponseViewModel<bool>>
    {
        public int Id { get; init; }
    }
}
