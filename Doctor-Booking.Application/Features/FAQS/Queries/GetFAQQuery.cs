using Doctor_Booking.Application.DTOs;
using Doctor_Booking.Application.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.FAQS.Queries
{
    public class GetFAQQuery : IRequest<ResponseViewModel<FAQDto>>
    {
        public int faqId { get; set; }
        public GetFAQQuery(int id)
        {
            faqId = id;
        }
    }
}
