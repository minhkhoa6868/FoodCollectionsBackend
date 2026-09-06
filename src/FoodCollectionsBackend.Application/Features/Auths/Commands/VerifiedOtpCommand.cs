using FoodCollectionsBackend.Application.Common.Models;
using FluentValidation;
using FoodCollectionsBackend.Application.Features.Auths.Models;
using FoodCollectionsBackend.Application.Interfaces.Auths;
namespace FoodCollectionsBackend.Application.Features.Auths.Commands;

public sealed record VerifiedOtpCommand(string Email, string Otp) : IRequest<Result<VerifiedOtpResponse?>>;

public class VerifiedOtpCommandValidator : AbstractValidator<VerifiedOtpCommand>
{
    public VerifiedOtpCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email address");

        RuleFor(x => x.Otp)
            .NotEmpty().WithMessage("OTP is required");
    }
}

public class VerifiedOtpCommandHandler(IAuthCommandService authCommandService) : IRequestHandler<VerifiedOtpCommand, Result<VerifiedOtpResponse?>>
{
    public async Task<Result<VerifiedOtpResponse?>> Handle(VerifiedOtpCommand request, CancellationToken cancellationToken)
    => await authCommandService.VerifiedOtpAsync(request, cancellationToken);
}
