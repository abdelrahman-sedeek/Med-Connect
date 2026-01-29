using Doctor_Booking.Application.Features.Doctors.Commands;
using Doctor_Booking.Application.Interfaces;
using Doctor_Booking.Application.ViewModels;
using Doctor_Booking.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.Doctors.CommandHandlers
{
    public class DeleteDoctorCommandHandler
        : IRequestHandler<DeleteDoctorCommand, ResponseViewModel<bool>>
    {
        private readonly IDoctorBookingDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<DeleteDoctorCommandHandler> _logger;

        public DeleteDoctorCommandHandler(
            IDoctorBookingDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<DeleteDoctorCommandHandler> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<ResponseViewModel<bool>> Handle(
            DeleteDoctorCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var doctor = await _context.Doctors
                    .Include(d => d.User)
                    .FirstOrDefaultAsync(d => d.Id == request.DoctorId, cancellationToken);

                if (doctor == null)
                {
                    return ResponseViewModel<bool>.FailureResponse(
                        message: "Doctor not found",
                        status: 404
                    );
                }

                // Soft delete: deactivate instead of removing
                doctor.User.IsActive = false;
                
                await _context.SaveChangesAsync(cancellationToken);

                return ResponseViewModel<bool>.SuccessResponse(
                    data: true,
                    message: "Doctor deleted successfully",
                    status: 200
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting doctor");
                return ResponseViewModel<bool>.FailureResponse(
                    message: "An error occurred while deleting doctor",
                    status: 500,
                    errors: new List<object> { ex.Message }
                );
            }
        }
    }

}
