using Doctor_Booking.Application.Features.FAQS.Commands;
using Doctor_Booking.Application.Interfaces.Repositories;
using Doctor_Booking.Application.ViewModels;
using Doctor_Booking.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.FAQS.CommandsHandler
{
    public class UpdateFAQCommandHandler : IRequestHandler<UpdateFAQCommand, ResponseViewModel<string>>
    {
        private readonly IFAQRepository _faqRepository;
        public UpdateFAQCommandHandler(IFAQRepository faqRepository)
        {
            _faqRepository = faqRepository;
        }

        public async Task<ResponseViewModel<string>> Handle(UpdateFAQCommand request, CancellationToken cancellationToken)
        {
            var faq = new FAQ()
            {
                Id=request.FAQDto.Id,
                Question = request.FAQDto.Question,
                Answer = request.FAQDto.Answer,
                IsActive = request.FAQDto.IsActive,
            };
            var res = await _faqRepository.UpdateAsync(faq);
            if (res == 0) return ResponseViewModel<string>.FailureResponse("There is an error while updating this question", 400, "There is an error while updating this question", null);
            return ResponseViewModel<string>.SuccessResponse();
        }
    }
}
