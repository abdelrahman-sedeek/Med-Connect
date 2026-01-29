using Doctor_Booking.Application.DTOs.Doctor;
using Doctor_Booking.Application.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.Doctors.Queries
{
    public record GetDoctorByIdQuery(int id):IRequest<ResponseViewModel<DoctorDto>>;
    
}
