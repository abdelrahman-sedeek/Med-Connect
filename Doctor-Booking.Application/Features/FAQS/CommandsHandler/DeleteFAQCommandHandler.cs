using Doctor_Booking.Application.Features.FAQS.Commands;
using Doctor_Booking.Application.Interfaces.Repositories;
using Doctor_Booking.Application.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.FAQS.CommandsHandler
{
    public class DeleteFAQCommandHandler : IRequestHandler<DeleteFAQCommand, ResponseViewModel<string>>
    {
        private readonly IFAQRepository _faqRepository;
        public DeleteFAQCommandHandler(IFAQRepository faqRepository)
        {
            _faqRepository = faqRepository;
        }
        public async Task<ResponseViewModel<string>> Handle(DeleteFAQCommand request, CancellationToken cancellationToken)
        {
            var res = await _faqRepository.DeleteAsync(request.Id);
            if (res == 0) return ResponseViewModel<string>.FailureResponse("There is an error while deleting this question", 400, "There is an error while deleting this question", null);
            return ResponseViewModel<string>.SuccessResponse();
        }
    }
}
