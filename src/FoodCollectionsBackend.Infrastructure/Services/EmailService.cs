using FoodCollectionsBackend.Application.Interfaces;
using FoodCollectionsBackend.Application.Common.Models;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Hosting;

namespace FoodCollectionsBackend.Infrastructure.Services;

public class EmailService(IOptions<EmailSettings> settings, IWebHostEnvironment environment) : IEmailService
{
    public async Task SendOtpAsync(string toEmail, string otp)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(settings.Value.FromName, settings.Value.FromEmail));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = "Your OTP Code";
        message.Body = new TextPart("html")
        {
            Text = $"""
                <h2>Reset Password</h2>
                <p>Your OTP code is: <strong>{otp}</strong></p>
                <p>Expires in 5 minutes.</p>
                """
        };

        using var client = new SmtpClient();

        if (environment.IsDevelopment())
        {
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;
        }

        await client.ConnectAsync(settings.Value.Host, settings.Value.Port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(settings.Value.Username, settings.Value.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
