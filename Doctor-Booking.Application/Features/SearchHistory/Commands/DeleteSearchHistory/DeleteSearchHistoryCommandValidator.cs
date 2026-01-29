using FluentValidation;

namespace Doctor_Booking.Application.Features.SearchHistory.Commands.DeleteSearchHistory
{
    public class DeleteSearchHistoryCommandValidator : AbstractValidator<DeleteSearchHistoryCommand>
    {
        public DeleteSearchHistoryCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Search history ID must be greater than 0.");
        }
    }
}
