using Doctor_Booking.Application.Interfaces.Repositories;
using Doctor_Booking.Application.ViewModels;
using Doctor_Booking.Domain.Entities;
using MediatR;

namespace Doctor_Booking.Application.Features.Specialties.Commands.DeleteSpecialty
{
    public class DeleteSpecialtyCommandHandler : IRequestHandler<DeleteSpecialtyCommand, ResponseViewModel<bool>>
    {
        private readonly ISpecialtyRepository specialtyRepository;
        public DeleteSpecialtyCommandHandler(ISpecialtyRepository specialtyRepository)
        {
            this.specialtyRepository = specialtyRepository;
        }


        public async Task<ResponseViewModel<bool>> Handle(DeleteSpecialtyCommand request, CancellationToken cancellationToken)
        {
            // Check if specialty exists
            var specialty = await specialtyRepository.GetSpecialtyByIdAsync(request.Id, cancellationToken);
            if (specialty == null)
            {
                return ResponseViewModel<bool>.FailureResponse("Specailty does not exists");
            }

            // Check if specialty has doctors
            var hasDoctors = await specialtyRepository.SpecialtyHasDoctorsAsync(request.Id, cancellationToken);
            if (hasDoctors)
            {
                return ResponseViewModel<bool>.FailureResponse(message: "Cannot delete specialty because it has doctors assigned to it", status: 409);
            }

            // Delete specialty
            var result = await specialtyRepository.DeleteSpecialtyAsync(request.Id, cancellationToken);
            return ResponseViewModel<bool>.SuccessResponse(result, message: "Specialty deleted successfully");
        }
    }
}
