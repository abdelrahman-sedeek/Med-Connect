using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.Specialties.Queries.GetAllSpecialties
{
    public record GetAllSpecialtiesQuery : IRequest<ResponseViewModel<List<SpecialtyDto>>>
    {
    }
}
