using Doctor_Booking.Application.Common.Models;
using Doctor_Booking.Application.Interfaces.Repositories;
using Doctor_Booking.Application.ViewModels;
using MediatR;

namespace Doctor_Booking.Application.Features.Doctors.Queries.SearchDoctors
{
    public class SearchDoctorsQueryHandler : IRequestHandler<SearchDoctorsQuery, ResponseViewModel<PaginatedList<SearchDoctorsDto>>>
    {
        private readonly IDoctorReadRepository doctorRepo;
        private readonly IPatientRepository patientRepo;
        private readonly ISearchHistoryRepository searchHistoryRepo;

        public SearchDoctorsQueryHandler(IDoctorReadRepository doctorRepo, IPatientRepository patientRepo, ISearchHistoryRepository searchHistoryRepo)
        {
            this.doctorRepo = doctorRepo;
            this.patientRepo = patientRepo;
            this.searchHistoryRepo = searchHistoryRepo;
        }


        public async Task<ResponseViewModel<PaginatedList<SearchDoctorsDto>>> Handle(SearchDoctorsQuery request, CancellationToken cancellationToken)
        {
            double? latitude = request.Latitude;
            double? longitude = request.Longitude;

            // Get location based on user type
            if (!latitude.HasValue && !longitude.HasValue)
            {
                if (request.PatientId.HasValue)
                {
                    // Get patient's location
                    var patientLocation = await patientRepo.GetPatientLocationAsync(
                        request.PatientId.Value,
                        cancellationToken
                    );

                    if (patientLocation != null)
                    {
                        latitude = patientLocation.Latitude;
                        longitude = patientLocation.Longitude;
                    }
                }
                else if (request.DoctorId.HasValue)
                {
                    // Get doctor's location
                    var doctorLocation = await doctorRepo.GetDoctorLocationAsync(
                        request.DoctorId.Value,
                        cancellationToken
                    );

                    if (doctorLocation != null)
                    {
                        latitude = doctorLocation.Latitude;
                        longitude = doctorLocation.Longitude;
                    }
                }
            }

            var result = await doctorRepo.SearchDoctorsAsync(
                request.Query,
                request.Specialty,
                latitude!.Value,
                longitude!.Value,
                request.RadiusKm,
                request.PageNumber,
                request.PageSize,
                cancellationToken
            );

            return ResponseViewModel<PaginatedList<SearchDoctorsDto>>.SuccessResponse(result);
        }
    }
}
