using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.Specialties.Commands.CreateSpecialty
{
    public record CreateSpecialtyCommand : IRequest<ResponseViewModel<int>>
    {
        public string Name { get; init; } = string.Empty;
    }
}
