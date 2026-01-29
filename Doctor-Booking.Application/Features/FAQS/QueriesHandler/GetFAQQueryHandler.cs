using Doctor_Booking.Application.DTOs;
using Doctor_Booking.Application.Features.FAQS.Queries;
using Doctor_Booking.Application.Interfaces.Repositories;
using Doctor_Booking.Application.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.FAQS.QueriesHandler
{
    public class GetFAQQueryHandler : IRequestHandler<GetFAQQuery,ResponseViewModel<FAQDto>>
    {
        private readonly IFAQRepository _fAQRepository;
        public GetFAQQueryHandler(IFAQRepository fAQRepository) { 
            _fAQRepository= fAQRepository;

        }

        public async Task<ResponseViewModel<FAQDto>> Handle(GetFAQQuery request, CancellationToken cancellationToken)
        {
            var fAQ =await _fAQRepository.GetAsync(request.faqId);
            var res = new FAQDto()
            {
                Id = fAQ.Id,
                Question = fAQ.Question,
                Answer = fAQ.Answer,
                IsActive = fAQ.IsActive,
            };
            return ResponseViewModel<FAQDto>.SuccessResponse(res, 200);
        }
    }
}
