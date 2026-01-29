using Doctor_Booking.Application.Interfaces.Repositories;
using Doctor_Booking.Domain.Entities;
using Doctor_Booking.Application.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doctor_Booking.Application.Features.FAQS.Commands;

namespace Doctor_Booking.Application.Features.FAQS.CommandsHandler
{
    public class AddFAQCommandHandler : IRequestHandler<AddFAQCommand, ResponseViewModel<string>>
    {
        private readonly IFAQRepository _faqRepository;
        public AddFAQCommandHandler(IFAQRepository faqRepository)
        {
            _faqRepository = faqRepository;
        }

        public async Task<ResponseViewModel<string>> Handle(AddFAQCommand request, CancellationToken cancellationToken)
        {
            var faq = new FAQ()
            {
                Question = request.FaqDto.Question,
                Answer = request.FaqDto.Answer,
                IsActive = request.FaqDto.IsActive,
            };
            var res = await _faqRepository.AddAsync(faq);
            if (res == 0) return  ResponseViewModel<string>.FailureResponse("There is an error while adding this question",400, "There is an error while adding this question",null);
            return ResponseViewModel<string>.SuccessResponse();
        }
    }
}
