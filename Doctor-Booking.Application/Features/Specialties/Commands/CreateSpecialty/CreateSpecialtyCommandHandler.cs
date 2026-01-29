using Doctor_Booking.Application.Interfaces.Repositories;
using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.Specialties.Commands.CreateSpecialty
{
    public class CreateSpecialtyCommandHandler : IRequestHandler<CreateSpecialtyCommand, ResponseViewModel<int>>
    {
        private readonly ISpecialtyRepository specialtyRepository;
        public CreateSpecialtyCommandHandler(ISpecialtyRepository specialtyRepository)
        {
            this.specialtyRepository = specialtyRepository;
        }


        public async Task<ResponseViewModel<int>> Handle(CreateSpecialtyCommand request, CancellationToken cancellationToken)
        {
            // Check if specialty already exists
            var exists = await specialtyRepository.SpecialtyExistsAsync(request.Name, cancellationToken);

            if (exists) 
                return ResponseViewModel<int>.FailureResponse(message: $"Specialty '{request.Name}' already exists", status: 409);

            int specialtyId = await specialtyRepository.CreateSpecialtyAsync(request.Name, cancellationToken);
            return ResponseViewModel<int>.SuccessResponse(data: specialtyId, status: 201, message: "Specialty created successfully");
        }
    }
}
