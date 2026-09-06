using FoodCollectionsBackend.Application.Common.Models;
using FluentValidation;
using FoodCollectionsBackend.Application.Interfaces.Auths;
namespace FoodCollectionsBackend.Application.Features.Auths.Commands;

public sealed record SendOtpCommand(string Email) : IRequest<Result>;

public class SendOtpCommandValidator : AbstractValidator<SendOtpCommand>
{
    public SendOtpCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email address");
    }
}

public class SendOtpCommandHandler(IAuthCommandService authCommandService) : IRequestHandler<SendOtpCommand, Result>
{
    public async Task<Result> Handle(SendOtpCommand request, CancellationToken cancellationToken)
    => await authCommandService.SendOtpAsync(request, cancellationToken);
}
