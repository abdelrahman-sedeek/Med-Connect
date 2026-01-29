using Doctor_Booking.Application.DTOs;
using Doctor_Booking.Application.Features.FAQS.Queries;
using Doctor_Booking.Application.Interfaces.Repositories;
using Doctor_Booking.Application.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.FAQS.QueriesHandler
{
    public class GetAllFAQSQueryHandler : IRequestHandler<GetAllFAQSQuery, ResponseViewModel<List<FAQDto?>>>
    {
        private readonly IFAQRepository _faqRepository;
        public GetAllFAQSQueryHandler(IFAQRepository faqRepository)
        {
            _faqRepository= faqRepository;
        }
        public async Task<ResponseViewModel<List<FAQDto?>>> Handle(GetAllFAQSQuery request, CancellationToken cancellationToken)
        {
            var Faqs =await _faqRepository.GetAllAsync();
            List<FAQDto?> res= new List<FAQDto?>();
            foreach(var faq in Faqs)
            {
                var resDto = new FAQDto()
                {
                    Id = faq.Id,
                    Question = faq.Question,
                    Answer = faq.Answer,
                    IsActive = faq.IsActive,
                };
                res.Add(resDto);
            }
            return ResponseViewModel<List<FAQDto?>>.SuccessResponse(res, 200, "Request completed succesfully");
        }
    }
}
