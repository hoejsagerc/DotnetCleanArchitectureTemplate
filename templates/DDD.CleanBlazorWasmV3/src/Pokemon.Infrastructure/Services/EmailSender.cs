using Microsoft.AspNetCore.Identity.UI.Services;

namespace Pokemon.Infrastructure.Services;

public class EmailSender : IEmailSender
{
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // use smtp client

        // use cloud services
    }
}
