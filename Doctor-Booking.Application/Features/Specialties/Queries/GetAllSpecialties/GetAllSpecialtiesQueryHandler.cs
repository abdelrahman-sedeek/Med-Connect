using Doctor_Booking.Application.Interfaces.Repositories;
using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.Specialties.Queries.GetAllSpecialties
{
    public class GetAllSpecialtiesQueryHandler : IRequestHandler<GetAllSpecialtiesQuery, ResponseViewModel<List<SpecialtyDto>>>
    {
        private readonly ISpecialtyRepository specialtyRepository;
        public GetAllSpecialtiesQueryHandler(ISpecialtyRepository specialtyRepository)
        {
            this.specialtyRepository = specialtyRepository;
        }


        public async Task<ResponseViewModel<List<SpecialtyDto>>> Handle(GetAllSpecialtiesQuery request, CancellationToken cancellationToken)
        {
            var specialties = await specialtyRepository.GetAllSpecialtyAsync(cancellationToken);
            return ResponseViewModel<List<SpecialtyDto>>.SuccessResponse(specialties);
        }
    }
}
