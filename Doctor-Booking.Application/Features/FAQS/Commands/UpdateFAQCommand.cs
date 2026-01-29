using Doctor_Booking.Application.DTOs;
using Doctor_Booking.Application.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.FAQS.Commands
{
    public class UpdateFAQCommand : IRequest<ResponseViewModel<string>>
    {
        public FAQDto FAQDto { get; set; }
        public UpdateFAQCommand(FAQDto FaqDto)
        {
            FAQDto = FaqDto;
        }
    }
}
