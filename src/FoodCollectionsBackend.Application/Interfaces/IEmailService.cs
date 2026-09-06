namespace FoodCollectionsBackend.Application.Interfaces;

public interface IEmailService
{
    Task SendOtpAsync(string toEmail, string otp);
}
