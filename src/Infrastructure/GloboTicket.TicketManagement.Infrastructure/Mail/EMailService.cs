using System.Net;
using GloboTicket.TicketManagement.Application.Contracts.Infrastructure;
using GloboTicket.TicketManagement.Application.Models.Mail;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace GloboTicket.TicketManagement.Infrastructure.Mail;

public class EMailService : IEmailService
{
    protected readonly EmailSettings _emailSettings;

    public EMailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task<bool> SendEmail(Email email)
    {
        var client = new SendGridClient(_emailSettings.ApiKey);

        var toEmailAddress = new EmailAddress
        {
            Email = email.To
        };
        var fromEmailAddress = new EmailAddress
        {
            Email = _emailSettings.FromAddress,
            Name = _emailSettings.FromName
        };

        var sendGridMessage =
            MailHelper.CreateSingleEmail(fromEmailAddress, toEmailAddress, email.Subject, email.Body, email.Body);
        var response = await client.SendEmailAsync(sendGridMessage);
        
        var isSuccessful = response.StatusCode is HttpStatusCode.Accepted or HttpStatusCode.OK;

        return isSuccessful;
    }
}