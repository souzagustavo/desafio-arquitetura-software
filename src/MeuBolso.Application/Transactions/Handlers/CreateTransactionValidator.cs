using FluentValidation;

namespace MeuBolso.Application.Transactions.Handlers
{
    public class CreateTransactionValidator : AbstractValidator<CreateTransactionRequest>
    {
        public CreateTransactionValidator()
        {
            RuleFor(x => x.AccountId)
                .NotEmpty().WithMessage("AccountId is required.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Type must be a valid transaction type.");
        }
    }
}
